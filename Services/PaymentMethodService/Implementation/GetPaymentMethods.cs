using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Services.PaymentMethodService;

public partial class PaymentMethodService : IPaymentMethodService
{
    public async Task<IEnumerable<PaymentMethod>> GetPaymentMethods(Guid userId)
    {
        _logger.LogDebug("Get payment methods of user {id}", userId);
        if (Guid.Empty == userId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new ArgumentException("User id is not a valid GUID", nameof(userId))
            );
        var entities = await _collection.GetPaymentMethods(elem => elem.UserId == userId);
        _logger.LogInformation(
            LogEvents.PaymentMethodReadSuccess,
            "Payment methods have been read successfully"
        );
        return entities.Select(elem => new PaymentMethod(elem));
    }
}
