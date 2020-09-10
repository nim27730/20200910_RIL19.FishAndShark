using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RIL19.FishAndShark.Core.Data;
using RIL19.FishAndShark.Core.Data.Decorations;
using RIL19.FishAndShark.Core.Data.Poissons;

namespace RIL19.FishAndShark.Mongodb
{
    public static class MongoDbServiceCollection
    {
        public static IServiceCollection AddMongoDBRepository(this IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<Aquarium>();
            BsonClassMap.RegisterClassMap<OFNI>(cm =>
            {
                cm.AutoMap();
                cm.UnmapMember(o => o.Aquarium);
            });
            BsonClassMap.RegisterClassMap<Poisson>();
            BsonClassMap.RegisterClassMap<Chat>();
            BsonClassMap.RegisterClassMap<Lune>();
            BsonClassMap.RegisterClassMap<Oeuf>();
            BsonClassMap.RegisterClassMap<Requin>();
            BsonClassMap.RegisterClassMap<Rouge>();
            BsonClassMap.RegisterClassMap<Decoration>();
            BsonClassMap.RegisterClassMap<Algue>();
            BsonClassMap.RegisterClassMap<Pierre>();


            #region DependencyInjection
            services.AddSingleton<IMongoClient>(context => new MongoClient("mongodb://localhost:27017"));
            services.AddSingleton<IMongoDatabase>(context => context.GetService<IMongoClient>().GetDatabase("RIL19_FishAndShark"));
            services.AddSingleton<IMongoCollection<Aquarium>>(context => context.GetService<IMongoDatabase>().GetCollection<Aquarium>("Aquariums").WithWriteConcern(WriteConcern.WMajority));
            services.AddSingleton<IAquariumRepository, AquariumRepository>();
            #endregion

            return services;
        }
    }

}
