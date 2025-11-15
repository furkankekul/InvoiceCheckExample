namespace Invoice.Business
{
    public interface IMockInvoiceService
    {
        public MockResponseModel GetMockResponse(string invoiceNumber, string taxNumber);
    }
}
