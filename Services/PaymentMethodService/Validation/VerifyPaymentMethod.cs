using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Services.PaymentMethodService;

public partial class PaymentMethodService : IPaymentMethodService
{
    /// <summary>
    /// This method verifies that an existing payment method is valid.
    /// </summary>
    /// <param name="method">
    /// The payment method to verify.
    /// </param>
    /// <returns>
    /// The payment method from the database, or <c>null</c> if it does not exist.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the payment method id is an empty GUID.
    /// If the user id is an empty GUID.
    /// If the payment method type is empty.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the payment method is not accessible by the user.
    /// </exception>
    /// <exception cref="EntityNotFoundException">
    /// If the payment method does not exist.
    /// </exception>
    private async Task<PaymentMethod> VerifyExistingPaymentMethod(PaymentMethod method)
    {
        _logger.LogDebug("Verify existing payment method {method}", method.ToString());
        var entity = await VerifyPaymentMethodAccess(method.Id, method.UserId);
        VerifyPaymentMethodType(method.Type);
        foreach (var instance in method.Instances)
        {
            VerifyPaymentInstance(instance);
        }
        return entity;
    }

    /// <summary>
    /// This method verifies that a new payment method is valid.
    /// </summary>
    /// <param name="method">
    /// The payment method to verify.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the payment method id is an empty GUID.
    /// If the user id is an empty GUID.
    /// If the payment method type is empty.
    /// </exception>
    /// <exception cref="DuplicateEntityException">
    /// If the payment method id does already exist.
    /// </exception>
    private async Task VerifyNewPaymentMethod(PaymentMethod method)
    {
        _logger.LogDebug("Verify new payment method {method}", method.ToString());
        var entity = await VerifyPaymentMethodId(method.Id);
        if (Guid.Empty == method.UserId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new ArgumentException("User id is not a valid GUID", nameof(method))
            );
        if (entity is not null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodInsertFailed,
                new DuplicateEntityException("Payment method does already exist")
            );
        VerifyPaymentMethodType(method.Type);
        foreach (var instance in method.Instances)
        {
            VerifyPaymentInstance(instance);
        }
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
    /// If the payment method id is an empty GUID.
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
    /// The payment method from the database.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the payment method id is an emtpy GUID.
    /// If the user id is an emtpy GUID.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the payment method is not accessible by the user.
    /// </exception>
    /// <exception cref="EntityNotFoundException">
    /// If the payment method does not exist.
    /// </exception>
    private async Task<PaymentMethod> VerifyPaymentMethodAccess(Guid methodId, Guid userId)
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
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new EntityNotFoundException("Payment method does not exist")
            );
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
    /// <param name="type">
    /// The name of the payment method to verify.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the payment method name is empty.
    /// </exception>
    private void VerifyPaymentMethodType(string type)
    {
        _logger.LogDebug("Verify payment method type {type}", type);
        if (string.IsNullOrWhiteSpace(type))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.PaymentMethodOperationFailed,
                new ArgumentException("Payment method type is empty", nameof(type))
            );
    }
}
