using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.Payment;

public partial class PaymentMethodServiceUnitTests
{
    [Fact]
    public async Task Should_FailGettingPaymentMethod_WhenIdIsEmpty()
    {
        var pm = _database.First();

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.GetPaymentMethod(Guid.Empty, pm.UserId)
        );
    }

    [Fact]
    public async Task Should_FailGettingPaymentMethod_WhenUserIdIsEmpty()
    {
        var pm = _database.First();

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.GetPaymentMethod(pm.Id, Guid.Empty)
        );
    }

    [Fact]
    public async Task Should_FailGettingPaymentMethod_WhenMethodIsNotAccessible()
    {
        var pm = _database.First();

        await Assert.ThrowsAsync<AuthorizationException>(
            () => _paymentMethodService.GetPaymentMethod(pm.Id, _paymentMethod.UserId)
        );
    }

    [Fact]
    public async Task Should_FailGettingPaymentMethod_WhenMethodDoesNotExist()
    {
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _paymentMethodService.GetPaymentMethod(Guid.NewGuid(), _paymentMethod.UserId)
        );
    }

    [Fact]
    public async Task Should_ReturnCopyOfRequestedPaymentMethod()
    {
        var pm = _database.First();
        var result = await _paymentMethodService.GetPaymentMethod(pm.Id, pm.UserId);

        Assert.NotSame(pm, result);
    }

    [Fact]
    public async Task Should_ReturnRequestedPaymentMethod()
    {
        var pm = _database.First();
        var result = await _paymentMethodService.GetPaymentMethod(pm.Id, pm.UserId);

        Assert.Equal(pm.Id, result.Id);
        Assert.Equal(pm.UserId, result.UserId);
        Assert.Equal(pm.Type, result.Type);
        Assert.Equal(pm.Instances.Count(), result.Instances.Count());
    }
}
