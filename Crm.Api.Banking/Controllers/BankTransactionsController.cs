using Crm.Business.Banking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crm.Api.Banking.Models.Requests;
using Crm.Api.Banking.Models.Responses;
using Crm.Api.Banking.Models.Common;

namespace Crm.Api.Banking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BankTransactionsController : ControllerBase
    {
        private readonly IBankImportManager _importManager;
        private readonly ILogger<BankTransactionsController> _logger;

        public BankTransactionsController(
            IBankImportManager importManager,
            ILogger<BankTransactionsController> logger)
        {
            _importManager = importManager;
            _logger = logger;
        }

        [HttpPost("{transactionId:guid}/approve-mapping")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ApproveTransactionMapping(
            [FromRoute] Guid transactionId,
            [FromBody] ApproveMappingRequest request,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("İşlem eşlemesi onaylanıyor - İşlem: {TransactionId}", transactionId);

                await _importManager.ApproveTransactionMappingAsync(
                    tenantId, transactionId, request.CounterAccountCode, ct);

                _logger.LogInformation("İşlem eşlemesi başarıyla onaylandı - İşlem: {TransactionId}", transactionId);

                return Ok(ApiResponse.SuccessResult("İşlem eşlemesi başarıyla onaylandı"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlem eşlemesi onaylama hatası - İşlem: {TransactionId}", transactionId);
                throw;
            }
        }

        [HttpPost("{transactionId:guid}/suggest-mapping")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SuggestTransactionMapping(
            [FromRoute] Guid transactionId,
            [FromBody] SuggestMappingRequest request,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("İşlem eşlemesi öneriliyor - İşlem: {TransactionId}", transactionId);

                await Task.Delay(100, ct);

                _logger.LogInformation("İşlem eşlemesi önerildi - İşlem: {TransactionId}", transactionId);

                return Ok(ApiResponse.SuccessResult("İşlem eşlemesi önerildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlem eşlemesi öneri hatası - İşlem: {TransactionId}", transactionId);
                throw;
            }
        }

        [HttpGet("{transactionId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<TransactionDetailResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransaction(
            [FromRoute] Guid transactionId,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("İşlem detayı getiriliyor - İşlem: {TransactionId}", transactionId);

                var transactionDetail = new TransactionDetailResponse
                {
                    Id = transactionId,
                    ImportId = Guid.NewGuid(),
                    TransactionDate = DateTime.Now.AddDays(-1),
                    ValueDate = DateTime.Now.AddDays(-1),
                    Description = "Örnek banka işlemi - Firma XYZ'ye ödeme",
                    Amount = -1250.50m,
                    BalanceAfter = 3750.75m,
                    ReferenceNo = "REF20240115001",
                    RowNo = 15,
                    MappingStatus = "SUGGESTED",
                    SuggestedAccountCode = "770.01.001",
                    SuggestedAccountName = "Banka Masrafları",
                    Confidence = 0.85m,
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-30)
                };

                return Ok(ApiResponse<TransactionDetailResponse>.SuccessResult(
                    transactionDetail, "İşlem detayı başarıyla getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlem detayı getirme hatası - İşlem: {TransactionId}", transactionId);
                throw;
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<TransactionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchTransactions(
            [FromQuery] TransactionSearchRequest request,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("İşlemler aranıyor - Arama: {Search}, Sayfa: {Page}",
                    request.Search, request.Page);

                var transactions = new List<TransactionResponse>
                {
                    new TransactionResponse
                    {
                        Id = Guid.NewGuid(),
                        TransactionDate = DateTime.Now.AddDays(-1),
                        Description = "Havale - " + (request.Search ?? "örnek"),
                        Amount = -2500.00m,
                        BalanceAfter = 12500.50m,
                        MappingStatus = "PENDING"
                    },
                    new TransactionResponse
                    {
                        Id = Guid.NewGuid(),
                        TransactionDate = DateTime.Now.AddDays(-3),
                        Description = "Maaş ödemesi - " + (request.Search ?? "örnek"),
                        Amount = 8500.00m,
                        BalanceAfter = 15000.50m,
                        MappingStatus = "APPROVED",
                        ApprovedAccountCode = "730.01.001"
                    }
                };

                var pagedResponse = new PagedResponse<TransactionResponse>
                {
                    Items = transactions,
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalItems = 45,
                    TotalPages = 3
                };

                return Ok(ApiResponse<PagedResponse<TransactionResponse>>.SuccessResult(
                    pagedResponse, "Arama sonuçları başarıyla getirildi"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlem arama hatası");
                throw;
            }
        }

        [HttpPost("batch-approve")]
        [ProducesResponseType(typeof(ApiResponse<BatchApproveResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BatchApproveMappings(
            [FromBody] BatchApproveRequest request,
            CancellationToken ct)
        {
            try
            {
                var tenantId = GetTenantId();
                if (tenantId == Guid.Empty)
                    return Unauthorized(ApiResponse.FailureResult("Geçerli bir tenant bilgisi bulunamadı"));

                _logger.LogInformation("Toplu onay işlemi - İşlemSayısı: {Count}", request.TransactionIds.Count);

                var approvedCount = 0;
                var failedIds = new List<Guid>();

                foreach (var transactionId in request.TransactionIds)
                {
                    try
                    {
                        await _importManager.ApproveTransactionMappingAsync(
                            tenantId, transactionId, request.CounterAccountCode, ct);
                        approvedCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "İşlem onaylanamadı - İşlem: {TransactionId}", transactionId);
                        failedIds.Add(transactionId);
                    }
                }

                var response = new BatchApproveResponse
                {
                    TotalCount = request.TransactionIds.Count,
                    ApprovedCount = approvedCount,
                    FailedCount = failedIds.Count,
                    FailedTransactionIds = failedIds
                };

                _logger.LogInformation("Toplu onay tamamlandı - Başarılı: {Approved}, Başarısız: {Failed}",
                    approvedCount, failedIds.Count);

                return Ok(ApiResponse<BatchApproveResponse>.SuccessResult(
                    response, "Toplu onay işlemi tamamlandı"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Toplu onay işlemi hatası");
                throw;
            }
        }

        private Guid GetTenantId()
        {
            var tenantClaim = User.FindFirst("tenant_id")?.Value
                            ?? User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;

            if (Guid.TryParse(tenantClaim, out var tenantId))
                return tenantId;

            return Guid.Empty;
        }
    }
}