namespace RIL19.FishAndShark.Core.Data
{
    public interface IMovable : IPositionable
    {
        int Speed { get; set; }
        double DX { get; set; }
        double DY { get; set; }
        void Move();
    }
}
