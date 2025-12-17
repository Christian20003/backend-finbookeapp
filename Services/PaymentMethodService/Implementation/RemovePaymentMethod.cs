using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Services.PaymentMethodService;

public partial class PaymentMethodService : IPaymentMethodService
{
    public async Task<PaymentMethod> RemovePaymentMethod(Guid methodId, Guid userId)
    {
        _logger.LogDebug("Remove payment method {id}", methodId);
        var entity = await _collection.GetPaymentMethod(elem => elem.Id == methodId);
        if (entity is null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodDeleteFailed,
                new EntityNotFoundException("Payment method does not exist")
            );
        if (entity.UserId != userId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodDeleteFailed,
                new AuthorizationException("Payment method is not accessible")
            );
        _collection.RemovePaymentMethod(entity);
        await _collection.SaveChanges();
        _logger.LogInformation(
            LogEvents.PaymentMethodDeleteSuccess,
            "Payment method has been removed successfully"
        );
        return new PaymentMethod(entity);
    }
}
