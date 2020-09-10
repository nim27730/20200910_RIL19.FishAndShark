using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Refit;
using RIL19.FishAndShark.Windows.Application.Data;

namespace RIL19.FishAndShark.Windows.Application.Services
{
    public interface IAquariumApi
    {
        [Get("/api/Aquarium/List")]
        Task<ObservableCollection<Aquarium>> ListAsync();

        [Get("/api/Aquarium")]
        Task<Aquarium> GetAsync(string id);

        [Post("/api/Aquarium/Create")]
        Task<Guid> CreateAsync(object createRequest);
    }
}
