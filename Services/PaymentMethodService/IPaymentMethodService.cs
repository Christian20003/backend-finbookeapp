using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Payment;
using Microsoft.EntityFrameworkCore;

namespace FinBookeAPI.Services.PaymentMethodService;

public interface IPaymentMethodService
{
    /// <summary>
    /// This method inserts a payment method to the database.
    /// </summary>
    /// <param name="method">
    /// The payment method to insert.
    /// </param>
    /// <returns>
    /// The inserted payment method.
    /// </returns>
    /// <exception cref="DuplicateEntityException">
    /// If the payment method id already exists.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the payment method is invalid.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the database operation is canceled
    /// by the application.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// If the database operation fails.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the database operation fails due to a concurrency conflict.
    /// </exception>
    public Task<PaymentMethod> CreatePaymentMethod(PaymentMethod method);

    /// <summary>
    /// This method updates a payment method in the database.
    /// </summary>
    /// <param name="method">
    /// The payment method to update with updated properties.
    /// </param>
    /// <returns>
    /// The updated payment method.
    /// </returns>
    /// <exception cref="EntityNotFoundException">
    /// If the payment method does not exist.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the payment method is invalid.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the payment method is not accessible.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the database operation is canceled
    /// by the application.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// If the database operation fails.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the database operation fails due to a concurrency conflict.
    /// </exception>
    public Task<PaymentMethod> UpdatePaymentMethod(PaymentMethod method);

    /// <summary>
    /// This method removes a payment method from the database.
    /// </summary>
    /// <param name="methodId">
    /// The id of the payment method to remove.
    /// </param>
    /// <param name="userId">
    /// The id of the user who owns the payment method.
    /// </param>
    /// <returns>
    /// The removed payment method.
    /// </returns>
    /// <exception cref="EntityNotFoundException">
    /// If the payment method does not exist.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the payment method id is empty.
    /// If the user id is empty.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the payment method is not accessible.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the database operation is canceled
    /// by the application.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// If the database operation fails.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If the database operation fails due to a concurrency conflict.
    /// </exception>
    public Task<PaymentMethod> RemovePaymentMethod(Guid methodId, Guid userId);

    /// <summary>
    /// This method gets a payment method from the database.
    /// </summary>
    /// <param name="methodId">
    /// The id of the payment method to get.
    /// </param>
    /// <param name="userId">
    /// The id of the user who owns the payment method.
    /// </param>
    /// <returns>
    /// The requested payment method.
    /// </returns>
    /// <exception cref="EntityNotFoundException">
    /// If the payment method does not exist.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the payment method id is empty.
    /// If the user id is empty.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the payment method is not accessible.
    /// </exception>
    public Task<PaymentMethod> GetPaymentMethod(Guid methodId, Guid userId);

    /// <summary>
    /// This method gets all payment methods of a user from the database.
    /// </summary>
    /// <param name="userId">
    /// The id of the user whose payment methods to get.
    /// </param>
    /// <returns>
    /// The requested payment methods.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the user id is empty.
    /// </exception>
    public Task<IEnumerable<PaymentMethod>> GetPaymentMethods(Guid userId);
}
