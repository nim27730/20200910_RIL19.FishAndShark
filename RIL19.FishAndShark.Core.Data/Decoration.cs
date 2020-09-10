using System;

namespace RIL19.FishAndShark.Core.Data
{
    public abstract class Decoration : OFNI
    {
        protected Random _rnd; 
        public Decoration(double x, double y, string color, double height, double width) : base(color, height, width)
        {
            _rnd = new Random(Guid.NewGuid().GetHashCode());
        }
        public Decoration(string color, double height, double width) : base(color, height, width)
        {
            _rnd = new Random(Guid.NewGuid().GetHashCode());
        }
        public override void InitializePosition()
        {
            X = _rnd.NextDouble() * Aquarium.Width;
            Y = 0;
        }
    }
}
