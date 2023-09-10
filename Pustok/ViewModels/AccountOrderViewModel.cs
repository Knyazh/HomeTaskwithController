namespace Pustok.ViewModels
{
    internal class AccountOrderViewModel
    {
        public DateTime CreatedOn { get; set; }
        public string OrderStatus { get; set; }
        public string TracingCode { get; set; }
        public object Total { get; set; }
        public int Count { get; set; }
        public string ImageURL { get; set; }

    }
}