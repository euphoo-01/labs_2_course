namespace L4;

public abstract partial class Product
{
    public GiftComponentType ComponentType { get; set; }
    public Dimensions Size { get; set; }
    public double Weight { get; set; }
}