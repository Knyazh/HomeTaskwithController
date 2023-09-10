namespace Pustok.Contracts
{
    public class CurrencyResult
    {
        public bool Success { get; set; }
        public long Timestamp { get; set; }
        public string Base { get; set; }
        public DateTime date { get; set; }

        public List<Rate> Rates { get; set; }


        public class Rate
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }

}
