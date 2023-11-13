using CryptoExchange.Net.Objects;

namespace WhiteBit.Net.Clients.Options
{
    public class WhiteBitEnvironment : TradeEnvironment
    {
        internal WhiteBitEnvironment(string name) : base(name)
        {
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static WhiteBitEnvironment Live { get; }
            = new WhiteBitEnvironment(TradeEnvironmentNames.Live);
    }
}