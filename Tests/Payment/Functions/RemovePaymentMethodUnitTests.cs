using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.Payment;

public partial class PaymentMethodServiceUnitTests
{
    [Fact]
    public async Task Should_FailRemovingPaymentMethod_WhenPaymentMethodDoesNotExist()
    {
        var pm = _database.First();

        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _paymentMethodService.RemovePaymentMethod(_paymentMethod.Id, pm.UserId)
        );
    }

    [Fact]
    public async Task Should_FailRemovingPaymentMethod_WhenPaymentMethodNotAccessible()
    {
        var pm = _database.First();

        await Assert.ThrowsAsync<AuthorizationException>(
            () => _paymentMethodService.RemovePaymentMethod(pm.Id, Guid.NewGuid())
        );
    }

    [Fact]
    public async Task Should_RemovePaymentMethodFromDatabase_WhenPaymentMethodDataIsValid()
    {
        var pm = _database.First();

        var result = await _paymentMethodService.RemovePaymentMethod(pm.Id, pm.UserId);

        Assert.DoesNotContain(_database, elem => elem.Id == pm.Id);
    }

    [Fact]
    public async Task Should_ReturnRemovedPaymentMethod_WhenPaymentMethodDataIsValid()
    {
        var pm = _database.First();

        var result = await _paymentMethodService.RemovePaymentMethod(pm.Id, pm.UserId);

        Assert.Equal(pm.Id, result.Id);
        Assert.Equal(pm.Type, result.Type);
        Assert.Equal(pm.Instances.Count, result.Instances.Count);
        foreach (var instance in result.Instances)
        {
            var dbpi = pm.Instances.First(elem => elem.Id == instance.Id);
            Assert.Equal(dbpi.Id, instance.Id);
            Assert.Equal(dbpi.Name, instance.Name);
            Assert.Equal(dbpi.Description, instance.Description);
        }
    }
}
