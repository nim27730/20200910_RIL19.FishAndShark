namespace RIL19.FishAndShark.Core.Data
{
    public interface IPositionable
    {
        Aquarium Aquarium { get; set; }
        double X { get; set; }
        double Y { get; set; }

        void InitializePosition();
        void InitializePosition(double x, double y);
    }
}
