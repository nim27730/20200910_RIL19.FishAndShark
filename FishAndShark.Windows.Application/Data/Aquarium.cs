using System;
using System.Collections.ObjectModel;

namespace RIL19.FishAndShark.Windows.Application.Data
{
    public class Aquarium
    {
        public Guid Id { get; set; }

        public double Height { get; set; }
        public double Width { get; set; }

        public string Name { get; set; }

        public ObservableCollection<OFNI> Elements { get; set; }
    }
}
