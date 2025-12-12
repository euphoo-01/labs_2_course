using System;

namespace L4
{
    [Serializable]
    public abstract class Sweet : Food
    {
        public string Cooker { get; set; }
        public int SugarPercent { get; set; }

        public abstract void Bake();

        public override string ToString()
        {
            return base.ToString() + $", Повар: {Cooker}, Сахар: {SugarPercent}%";
        }
    }
}
