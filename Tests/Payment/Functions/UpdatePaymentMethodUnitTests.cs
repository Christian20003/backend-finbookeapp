using FinBookeAPI.Models.Exceptions;
using FinBookeAPI.Models.Payment;

namespace FinBookeAPI.Tests.Payment;

public partial class PaymentMethodServiceUnitTests
{
    [Fact]
    public async Task Should_FailUpdatingPaymentMethod_WhenIdIsEmpty()
    {
        var pm = new PaymentMethod(_database.First()) { Id = Guid.Empty };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.UpdatePaymentMethod(pm)
        );
    }

    [Fact]
    public async Task Should_FailUpdatingPaymentMethod_WhenPaymentMethodDoesNotExist()
    {
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _paymentMethodService.UpdatePaymentMethod(_paymentMethod)
        );
    }

    [Fact]
    public async Task Should_FailUpdatingPaymentMethod_WhenUserIdIsEmpty()
    {
        var pm = new PaymentMethod(_database.First()) { UserId = Guid.Empty };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.UpdatePaymentMethod(pm)
        );
    }

    [Fact]
    public async Task Should_FailUpdatingPaymentMethod_WhenTypeIsInvalid()
    {
        var pm = new PaymentMethod(_database.First()) { Type = "" };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.UpdatePaymentMethod(pm)
        );
    }

    [Fact]
    public async Task Should_FailUpdatingPaymentMethod_WhenPaymentInstanceIdIsEmpty()
    {
        var pm = new PaymentMethod(_database.First())
        {
            Instances = [new PaymentInstance { Id = Guid.Empty }],
        };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.UpdatePaymentMethod(pm)
        );
    }

    [Fact]
    public async Task Should_FailUpdatingPaymentMethod_WhenPaymentInstanceDetailsAreInvalid()
    {
        var pm = new PaymentMethod(_database.First()) { Instances = [new PaymentInstance()] };

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.UpdatePaymentMethod(pm)
        );
    }

    [Fact]
    public async Task Should_StoreUpdateOnDatabase_WhenPaymentMethodDataIsValid()
    {
        var pm = new PaymentMethod(_database.First())
        {
            Type = "Hello World",
            Instances = [new PaymentInstance { Details = "Hello World" }],
        };

        var result = await _paymentMethodService.UpdatePaymentMethod(pm);
        var updatedpm = _database.First();

        Assert.Equal(pm.Type, updatedpm.Type);
        Assert.Equal(pm.Instances.Count(), updatedpm.Instances.Count());
        Assert.Equal(pm.Instances.First().Id, updatedpm.Instances.First().Id);
        Assert.Equal(pm.Instances.First().Details, updatedpm.Instances.First().Details);
    }

    [Fact]
    public async Task Should_ReturnUpdatedPaymentMethod_WhenPaymentMethodDataIsValid()
    {
        var pm = new PaymentMethod(_database.First())
        {
            Type = "Hello World",
            Instances = [new PaymentInstance { Details = "Hello World" }],
        };

        var result = await _paymentMethodService.UpdatePaymentMethod(pm);

        Assert.Equal(pm.Type, result.Type);
        Assert.Equal(pm.Instances.Count(), result.Instances.Count());
        Assert.Equal(pm.Instances.First().Id, result.Instances.First().Id);
        Assert.Equal(pm.Instances.First().Details, result.Instances.First().Details);
    }

    [Fact]
    public async Task Should_StoreCopyOfUpdatedPaymentMethod_WhenPaymentMethodDataIsValid()
    {
        var pm = new PaymentMethod(_database.First())
        {
            Type = "Hello World",
            Instances = [new PaymentInstance { Details = "Hello World" }],
        };

        var result = await _paymentMethodService.UpdatePaymentMethod(pm);
        var updatedpm = _database.First();

        Assert.NotSame(pm, updatedpm);
        Assert.NotSame(pm.Type, updatedpm.Type);
        Assert.NotSame(pm.Instances.First(), updatedpm.Instances.First());
        Assert.NotSame(pm.Instances.First().Details, updatedpm.Instances.First().Details);

        Assert.NotSame(result, updatedpm);
        Assert.NotSame(result.Type, updatedpm.Type);
        Assert.NotSame(result.Instances.First(), updatedpm.Instances.First());
        Assert.NotSame(result.Instances.First().Details, updatedpm.Instances.First().Details);
    }
}
