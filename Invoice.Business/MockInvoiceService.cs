namespace Invoice.Business
{
    public class MockInvoiceService : IMockInvoiceService
    {
        private readonly List<MockResponseModel> _mockData = new()
        {
            new MockResponseModel
            {
                InvoiceNumber = "FAT20251411001",
                TaxNumber = "1234567890",
                ResponseCode = "APPROVED",
                Message = "Fatura onaylandı"
            },
            new MockResponseModel
            {
                InvoiceNumber = "FAT20251411002",
                TaxNumber = "1234567890",
                ResponseCode = "REJECTED",
                Message = "Hatalı imza"
            }
        };

        public MockResponseModel GetMockResponse(string invoiceNumber, string taxNumber)
        {
            var response = _mockData.FirstOrDefault(x =>
                x.TaxNumber == taxNumber &&
                x.InvoiceNumber == invoiceNumber);

            return response ?? new MockResponseModel
            {
                InvoiceNumber = invoiceNumber,
                TaxNumber = taxNumber,
                ResponseCode = "NOT_FOUND",
                Message = "Fatura bulunamadı"
            };
        }
    }
}
