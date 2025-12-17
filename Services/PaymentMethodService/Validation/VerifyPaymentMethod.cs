using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Services.PaymentMethodService;

public partial class PaymentMethodService : IPaymentMethodService
{
    /// <summary>
    /// This method verifies that the payment method is valid.
    /// </summary>
    /// <param name="method">
    /// The payment method to verify.
    /// </param>
    /// <returns>
    /// The payment method from the database, or <c>null</c> if it does not exist.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the payment method id is not a valid GUID.
    /// If the payment method name is empty.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the payment method is not accessible by the user.
    /// </exception>
    private async Task<PaymentMethod?> VerifyPaymentMethod(PaymentMethod method)
    {
        _logger.LogDebug("Verify payment method {method}", method.ToString());
        var entity = await VerifyPaymentMethodAccess(method.Id, method.UserId);
        VerifyPaymentMethodName(method.Name);
        foreach (var instance in method.Instances)
        {
            VerifyPaymentInstance(instance);
        }
        return entity;
    }

    /// <summary>
    /// This method verifies that the payment method id is valid.
    /// </summary>
    /// <param name="methodId">
    /// The id of the payment method to verify.
    /// </param>
    /// <returns>
    /// The payment method from the database, or <c>null</c> if it does not exist.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the payment method id is not a valid GUID.
    /// </exception>
    private async Task<PaymentMethod?> VerifyPaymentMethodId(Guid methodId)
    {
        _logger.LogDebug("Verify payment method id {id}", methodId);
        if (Guid.Empty == methodId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new ArgumentException("Payment method id is not a valid GUID", nameof(methodId))
            );
        return await _collection.GetPaymentMethod(elem => elem.Id == methodId);
    }

    /// <summary>
    /// This method verifies that the payment method is accessible by the user.
    /// </summary>
    /// <param name="methodId">
    /// The id of the payment method to verify.
    /// </param>
    /// <param name="userId">
    /// The id of the user who owns the payment method.
    /// </param>
    /// <returns>
    /// The payment method from the database, or <c>null</c> if it does not exist.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the payment method id is not a valid GUID.
    /// If the user id is not a valid GUID.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the payment method is not accessible by the user.
    /// </exception>
    private async Task<PaymentMethod?> VerifyPaymentMethodAccess(Guid methodId, Guid userId)
    {
        _logger.LogDebug(
            "Verify payment method access for id {id} and user {userId}",
            methodId,
            userId
        );
        var entity = await VerifyPaymentMethodId(methodId);
        if (Guid.Empty == userId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new ArgumentException("User id is not a valid GUID", nameof(userId))
            );
        if (entity is null)
            return null;
        if (entity.UserId != userId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new AuthorizationException("Payment method is not accessible")
            );
        return entity;
    }

    /// <summary>
    /// This method verifies that the payment method name is valid.
    /// </summary>
    /// <param name="name">
    /// The name of the payment method to verify.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the payment method name is empty.
    /// </exception>
    private void VerifyPaymentMethodName(string name)
    {
        _logger.LogDebug("Verify payment method name {name}", name);
        if (string.IsNullOrWhiteSpace(name))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new ArgumentException("Payment method name is empty", nameof(name))
            );
    }
}
