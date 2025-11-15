namespace Invoice.Entites
{
    public class InvoiceStatusLog
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string ResponseCode { get; set; } = string.Empty;
        public string ResponseMessage { get; set; } = string.Empty;
        public DateTime RequestTime { get; set; } = DateTime.UtcNow;
    }
}
