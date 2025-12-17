using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Services.PaymentMethodService;

public partial class PaymentMethodService : IPaymentMethodService
{
    public async Task<PaymentMethod> GetPaymentMethod(Guid methodId, Guid userId)
    {
        _logger.LogDebug("Get payment method {id}", methodId);
        var entity = await VerifyPaymentMethodAccess(methodId, userId);
        if (entity is null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodReadFailed,
                new EntityNotFoundException("Payment method does not exist")
            );
        _logger.LogInformation(
            LogEvents.PaymentMethodReadSuccess,
            "Payment method has been read successfully"
        );
        return new PaymentMethod(entity);
    }
}
