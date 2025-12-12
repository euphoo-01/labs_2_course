namespace L4
{
    public interface IPurchasable
    {
        string Name { get; set; }
        decimal Price { get; set; }
        void Buy(Person p);
        void Sell(Person p);
        string GetDescription();
    }
}
