using System;

namespace RIL19.FishAndShark.Core.Data
{
    public abstract class Poisson : OFNI, IMovable
    {
        protected Random _rnd;
        public int Speed { get; set; }
        public double DX { get; set; }
        public double DY { get; set; }

        protected Poisson(int speed, string color, double height, double width) : base(color, height, width)
        {
            Speed = speed;
            _rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        public override void InitializePosition()
        {
            X = _rnd.NextDouble() * Aquarium.Width;
            Y = _rnd.NextDouble() * Aquarium.Height;
        }

        public virtual void Move()
        {
            throw new NotImplementedException();
        }
    }
}
