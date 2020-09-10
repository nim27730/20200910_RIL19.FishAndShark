using System;
using System.Collections.Generic;

namespace RIL19.FishAndShark.Core.Data
{
    public class Aquarium
    {
        public Guid Id { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public string Name { get; set; }
        public List<IPositionable> Elements { get; set; }

        public Aquarium()
        {
            Id = Guid.NewGuid();
            Elements = new List<IPositionable>();
        }
    }
}
