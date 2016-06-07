using Model;

namespace Common
{
    public static class LoggingExtensions
    {
        public static string LogFormat(this Bond bond)
        {
            return $"Bond : Identifier = {bond.Identifier}, Name = {bond.Name}, Type = {bond.Type}" +
                   (bond.MidPrice.HasValue ? $", Price = {bond.MidPrice.Value}" : string.Empty) +
                   (bond.Spread == null ? string.Empty : 
                       ", Spread { " +
                         (bond.Spread.Upper.HasValue ? $"Upper = {bond.Spread.Upper.Value}" : string.Empty) + 
                         (bond.Spread.Lower.HasValue ? (bond.Spread.Upper.HasValue ? ", " : string.Empty) + $"Lower = {bond.Spread.Lower.Value}" : string.Empty) +
                         " }");
        }
    }
}