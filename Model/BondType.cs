using EnergyTrading.Json;

namespace Model
{
    public class BondType : StringEnumClassImpl<BondType>
    {
        public static BondType Government = new BondType("Government");
        public static BondType Corporate = new BondType("Corporate");
        public static BondType ZeroCoupon = new BondType("Zero Coupon");
        public static BondType Unknown = new BondType("Unknown");

        private BondType(string value) : base(value)
        {
        }

    }
}