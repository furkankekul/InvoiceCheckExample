using Invoice.Business;
using Invoice.DataAccess.DataAccessBase.RepositoryDp;
using Invoice.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InvoiceCheckExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IRepository<InvoiceStatusLog> _invoiceStatusLogDataAccessor;
        private readonly IMockInvoiceService _mockInvoiceService;
        private readonly IMemoryCache _cache;
        public InvoiceController(IRepository<InvoiceStatusLog> invoiceStatusLogDataAccessor, IMockInvoiceService mockInvoiceService, IMemoryCache cache)
        {
            _invoiceStatusLogDataAccessor = invoiceStatusLogDataAccessor;
            _mockInvoiceService = mockInvoiceService;
            _cache = cache;
        }

        [HttpPost]
        public IActionResult InvoiceCheck(string invoiceNumber = null, string taxNumber = null)
        {
            InvoiceStatusLog invoiceStatusLog = new();
            string correlationId = Guid.NewGuid().ToString();

            if (invoiceNumber == null || taxNumber == null)
            {
                return BadRequest("Fatura numarası veya VKN boş olamaz.");
            }

            var responseModel = _mockInvoiceService.GetMockResponse(invoiceNumber, taxNumber);

            string cacheKey = $"{taxNumber}-{invoiceNumber}";
            invoiceStatusLog.InvoiceNumber = invoiceNumber;
            invoiceStatusLog.TaxNumber = taxNumber;

            if (!_cache.TryGetValue(cacheKey, out var mockResponseObj))
            {
                _cache.Set(cacheKey, responseModel, TimeSpan.FromMinutes(1));
            }

            var lastLog = _invoiceStatusLogDataAccessor.GetByPredicate(p => p.TaxNumber.Equals(taxNumber) && p.InvoiceNumber.Equals(invoiceNumber));

            if (lastLog != null && lastLog.ResponseCode.Equals(responseModel.ResponseCode))
            {

                invoiceStatusLog.ResponseCode = "BLOCKED";
                invoiceStatusLog.ResponseMessage = "Bu faturaya ait art arda 2 red cevabı alındı. Manuel inceleme gerekiyor.";

                _invoiceStatusLogDataAccessor.Add(invoiceStatusLog);
                Console.WriteLine($@"
                CorrelationId: {correlationId}, 
                <-------------------------------------------------->
                Request: 
                invoiceNumber: {invoiceStatusLog.InvoiceNumber}, 
                taxNumber: {taxNumber}, 
                <-------------------------------------------------->
                Mock Response: 
                ResponseCode: {invoiceStatusLog.ResponseCode},
                ResponseMessage: {invoiceStatusLog.ResponseMessage}
                <-------------------------------------------------->
                DB Log:
                InvoiceNumber: {invoiceStatusLog.InvoiceNumber};
                ResponseCode: {invoiceStatusLog.ResponseCode};
                ResponseMessage: {invoiceStatusLog.ResponseMessage};
                DateTime: {invoiceStatusLog.RequestTime} 
                ");
                return Ok(invoiceStatusLog);
            }

            invoiceStatusLog.ResponseCode = responseModel.ResponseCode;
            invoiceStatusLog.ResponseMessage = responseModel.Message;

            _invoiceStatusLogDataAccessor.Add(invoiceStatusLog);
            Console.WriteLine($@"
                CorrelationId: {correlationId}, 
                <-------------------------------------------------->
                Request: 
                invoiceNumber: {invoiceStatusLog.InvoiceNumber}, 
                taxNumber: {taxNumber}, 
                <-------------------------------------------------->
                Mock Response: 
                ResponseCode: {invoiceStatusLog.ResponseCode},
                ResponseMessage: {invoiceStatusLog.ResponseMessage}
                <-------------------------------------------------->
                DB Log:
                InvoiceNumber: {invoiceStatusLog.InvoiceNumber};
                ResponseCode: {invoiceStatusLog.ResponseCode};
                ResponseMessage: {invoiceStatusLog.ResponseMessage};
                DateTime: {invoiceStatusLog.RequestTime} 
                ");
            return Ok(invoiceStatusLog);

        }
    }
}
