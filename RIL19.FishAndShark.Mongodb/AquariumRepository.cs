using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using RIL19.FishAndShark.Core.Data;

namespace RIL19.FishAndShark.Mongodb
{
    public class AquariumRepository : IAquariumRepository
    {
        private readonly IMongoCollection<Aquarium> _aquariums;
        public AquariumRepository(IMongoCollection<Aquarium> aquariums)
        {
            _aquariums = aquariums;
        }

        public async Task Store(Aquarium data, CancellationToken cancellationToken)
        {
            var options = new FindOneAndReplaceOptions<Aquarium> { IsUpsert = true, ReturnDocument = ReturnDocument.After };
            await _aquariums.FindOneAndReplaceAsync<Aquarium>(c => c.Id == data.Id, data, options, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Aquarium data, CancellationToken cancellationToken)
        {
            await _aquariums.DeleteOneAsync(c => c.Id == data.Id, cancellationToken);
        }

        public async Task<Aquarium> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var aquarium = await _aquariums.Find(c => c.Id == id).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
            InitAquarium(aquarium);
            return aquarium;
        }

        public async Task<List<Aquarium>> ListAsync(Expression<Func<Aquarium, bool>> expression, CancellationToken cancellationToken)
        {
            var list = await _aquariums.Find(expression).ToListAsync(cancellationToken).ConfigureAwait(false);
            foreach (var aquarium in list)
                InitAquarium(aquarium);

            return list;
        }

        private void InitAquarium(Aquarium aquarium)
        {
            foreach (var element in aquarium.Elements)
                element.Aquarium = aquarium;
        }
    }

}
