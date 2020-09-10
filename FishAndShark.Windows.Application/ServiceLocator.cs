using System;
using Microsoft.Extensions.DependencyInjection;

namespace RIL19.FishAndShark.Windows.Application
{
    public static class ServicesLocator
    {
        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
