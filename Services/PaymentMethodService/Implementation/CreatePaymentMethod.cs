using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Services.PaymentMethodService;

public partial class PaymentMethodService : IPaymentMethodService
{
    public async Task<PaymentMethod> CreatePaymentMethod(PaymentMethod method)
    {
        _logger.LogDebug("Create new payment method {method}", method.ToString());
        var entity = await VerifyPaymentMethod(method);
        if (entity is not null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodInsertFailed,
                new DuplicateEntityException("Payment method does already exist")
            );
        var obj = new PaymentMethod(method);
        _collection.AddPaymentMethod(obj);
        await _collection.SaveChanges();
        _logger.LogInformation(
            LogEvents.PaymentMethodInsertSuccess,
            "Payment method has been created successfully"
        );
        return method;
    }
}
