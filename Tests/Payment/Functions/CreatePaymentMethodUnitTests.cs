using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.Payment;

public partial class PaymentMethodServiceUnitTests
{
    [Fact]
    public async Task Should_FailCreatingPaymentMethod_WhenIdIsEmpty()
    {
        _paymentMethod.Id = Guid.Empty;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.CreatePaymentMethod(_paymentMethod)
        );
    }

    [Fact]
    public async Task Should_FailCreatingPaymentMethod_WhenIdExist()
    {
        _paymentMethod.Id = _database.First().Id;

        await Assert.ThrowsAsync<DuplicateEntityException>(
            () => _paymentMethodService.CreatePaymentMethod(_paymentMethod)
        );
    }

    [Fact]
    public async Task Should_FailCreatingPaymentMethod_WhenUserIdIsEmpty()
    {
        _paymentMethod.UserId = Guid.Empty;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.CreatePaymentMethod(_paymentMethod)
        );
    }

    [Fact]
    public async Task Should_FailCreatingPaymentMethod_WhenTypeIsInvalid()
    {
        _paymentMethod.Type = string.Empty;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.CreatePaymentMethod(_paymentMethod)
        );
    }

    [Fact]
    public async Task Should_FailCreatingPaymentMethod_WhenPaymentInstanceIdIsEmpty()
    {
        _paymentMethod.Instances.First().Id = Guid.Empty;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.CreatePaymentMethod(_paymentMethod)
        );
    }

    [Fact]
    public async Task Should_FailCreatingPaymentMethod_WhenPaymentInstanceDetailsAreInvalid()
    {
        _paymentMethod.Instances.First().Details = string.Empty;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.CreatePaymentMethod(_paymentMethod)
        );
    }

    [Fact]
    public async Task Should_StoreCreatedPaymentMethod_WhenDataIsValid()
    {
        var result = await _paymentMethodService.CreatePaymentMethod(_paymentMethod);

        Assert.Contains(_database, pm => pm.Id == result.Id);
    }

    [Fact]
    public async Task Should_StoreCopyOfCreatedPaymentMethod()
    {
        var result = await _paymentMethodService.CreatePaymentMethod(_paymentMethod);
        var stored = _database.First(pm => pm.Id == result.Id);

        Assert.NotSame(stored, result);
    }

    [Fact]
    public async Task Should_ReturnCreatedPaymentMethod_WhenDataIsValid()
    {
        var result = await _paymentMethodService.CreatePaymentMethod(_paymentMethod);

        Assert.Equal(_paymentMethod.Id, result.Id);
        Assert.Equal(_paymentMethod.UserId, result.UserId);
        Assert.Equal(_paymentMethod.Type, result.Type);
        Assert.Equal(_paymentMethod.Instances.Count(), result.Instances.Count());
    }
}
