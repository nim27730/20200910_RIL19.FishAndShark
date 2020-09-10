using System;
using System.Threading.Tasks;

namespace RIL19.FishAndShark.API.Hubs
{
    public interface IAquariumLive
    {
        Task AquariumCreated(Guid aquariumId);
    }
}