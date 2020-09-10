using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using RIL19.FishAndShark.Core.Data;

namespace RIL19.FishAndShark.Mongodb
{
    public interface IAquariumRepository
    {
        Task Store(Aquarium data, CancellationToken cancellationToken);
        Task DeleteAsync(Aquarium data, CancellationToken cancellationToken);
        Task<Aquarium> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Aquarium>> ListAsync(Expression<Func<Aquarium, bool>> expression, CancellationToken cancellationToken);
    }
}