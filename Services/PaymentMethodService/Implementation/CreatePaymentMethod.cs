using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Services.PaymentMethodService;

public partial class PaymentMethodService : IPaymentMethodService
{
    public async Task<PaymentMethod> CreatePaymentMethod(PaymentMethod method)
    {
        _logger.LogDebug("Create new payment method {method}", method.ToString());
        await VerifyNewPaymentMethod(method);
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
