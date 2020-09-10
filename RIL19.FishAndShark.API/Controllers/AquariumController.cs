using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using RIL19.FishAndShark.Core.Data;
using RIL19.FishAndShark.Core.Data.Decorations;
using RIL19.FishAndShark.Core.Data.Poissons;
using RIL19.FishAndShark.Mongodb;
using Serilog;

namespace RIL19.FishAndShark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AquariumController : ControllerBase
    {
        private IAquariumRepository _repository;
        private readonly ILogger _logger;
        private readonly HubConnection _connection;
        public AquariumController(IAquariumRepository repository, ILogger logger, HubConnection connection)
        {
            _repository = repository;
            _logger = logger;
            _connection = connection;
        }

        [HttpGet("List")]
        public async Task<List<Aquarium>> List(CancellationToken cancellationToken)
        {
            try
            {
                var aquariums = await _repository.ListAsync((aquarium) => true, cancellationToken);
                return aquariums;
            }
            catch (Exception e)
            {
                _logger.Error(e, "List");
                return null;
            }
        }

        [HttpGet]
        public async Task<Aquarium> GetById(string id, CancellationToken cancellationToken)
        {
            try
            {
                var guid = Guid.Parse(id);
                var aquarium = await _repository.GetAsync(guid, cancellationToken);
                return aquarium;
            }
            catch (Exception e)
            {
                _logger.Error(e, "GetById");
                return null;
            }
        }

        [HttpPost("Create")]
        public async Task<Guid> Create(CreateRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var aquarium = new Aquarium() { Height = request.Height, Width = request.Width, Name = request.Name };
                aquarium.Elements.Add(new Chat());
                aquarium.Elements.Add(new Algue());
                foreach (var element in aquarium.Elements)
                {
                    element.Aquarium = aquarium;
                    element.InitializePosition();
                }
                await _repository.Store(aquarium, cancellationToken);

                await RaiseAquariumCreatedEvent(aquarium, cancellationToken);

                return aquarium.Id;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Create");
                return Guid.Empty;
            }
        }

        private async Task RaiseAquariumCreatedEvent(Aquarium aquarium, CancellationToken cancellationToken)
        {
            try
            {
                await _connection.InvokeAsync("SendAquariumCreatedMessage", aquarium.Id, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"RaiseAquariumCreatedEvent error [aquariumId:{aquarium?.Id}");
            }
        }
    }

    public class CreateRequest
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public string Name { get; set; }
    }
}
