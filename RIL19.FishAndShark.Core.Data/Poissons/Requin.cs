using System;

namespace RIL19.FishAndShark.Core.Data.Poissons
{
    public class Requin : Poisson
    {
        public Requin() : base(3,"", 10, 10)
        {
        }
        public override void Move()
        {
            throw new NotImplementedException();
        }

    }
}
