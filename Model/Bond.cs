namespace Model
{
    public class Bond
    {
        private BondType type = BondType.Unknown;
        public string Name { get; set; }

        public string Type
        {
            get { return type.ToString(); }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    type = BondType.Unknown;
                    return;
                }
                type = BondType.FromString(value);
            }
        }

        public decimal? MidPrice { get; set; }

        public PriceSpread Spread { get; set; }

        public string Identifier { get; set; }
    }
}