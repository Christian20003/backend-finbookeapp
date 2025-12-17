using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Services.PaymentMethodService;

public partial class PaymentMethodService : IPaymentMethodService
{
    /// <summary>
    /// This method verifies that the payment instance is valid.
    /// </summary>
    /// <param name="instance">
    /// The payment instance to verify.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the payment instance id is not a valid GUID.
    /// If the payment instance name is empty.
    /// </exception>
    private void VerifyPaymentInstance(PaymentInstance instance)
    {
        _logger.LogDebug("Verify payment instance {instance}", instance.ToString());
        if (Guid.Empty == instance.Id)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new ArgumentException("Payment instance id is not a valid GUID", nameof(instance))
            );
        if (string.IsNullOrWhiteSpace(instance.Name))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new ArgumentException("Payment instance name is empty", nameof(instance))
            );
    }
}
