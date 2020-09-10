using System;

namespace RIL19.FishAndShark.Core.Data.Poissons
{
    public class Oeuf : Poisson
    {
        public Oeuf() : base(3, "", 5, 5)
        {
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

    }
}
