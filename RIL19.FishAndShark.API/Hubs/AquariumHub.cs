using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace RIL19.FishAndShark.API.Hubs
{
    public class AquariumHub : Hub<IAquariumLive>
    {
        private readonly ILogger _logger;
        public AquariumHub(ILogger logger)
        {
            _logger = logger;
        }

        public async Task SendAquariumCreatedMessage(Guid aquariumId)
        {
            try
            {
                await Clients.All.AquariumCreated(aquariumId);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"SendAquariumCreatedMessage error [AquariumId:{aquariumId}]");
            }
        }

        #region Subscribe

        private string GetGroupName(Guid aquariumId)
        {
            return $"{aquariumId}";
        }
        public async Task SuscribeToAquarium(Guid aquarium)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(aquarium));
        }
        public async Task UnsuscribeToAquarium(Guid aquarium)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(aquarium));
        }

        #endregion

    }
}
