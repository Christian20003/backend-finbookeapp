using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.Payment;

public partial class PaymentMethodServiceUnitTests
{
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
        Assert.NotSame(pm.Type, result.Type);
        foreach (var instance in result.Instances)
        {
            var pi = pm.Instances.First(elem => elem.Id == instance.Id);
            Assert.NotSame(pi, instance);
            Assert.NotSame(pi.Name, instance.Name);
            if (pi.Description is not null)
                Assert.NotSame(pi.Description, instance.Description);
        }
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
