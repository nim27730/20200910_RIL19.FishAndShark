using System;

namespace RIL19.FishAndShark.Core.Data
{
    public abstract class OFNI : IPositionable, IDisplayable
    {
        public OFNI(string color, double height, double width)
        {
            Color = color;
            Height = height;
            Width = width;
        }
        #region Position
        public Aquarium Aquarium { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public abstract void InitializePosition();

        public void InitializePosition(double x, double y)
        {
            if (x > Aquarium.Width || y > Aquarium.Height || x < 0 || y < 0)
                throw new ArgumentException("Les valeurs de positions sont définies en dehors des limites de l'aquarium");
            X = x;
            Y = y;
        }
        #endregion

        #region Display
        public string Color { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        #endregion
    }
}
