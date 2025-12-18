namespace FinBookeAPI.Tests.Payment;

public partial class PaymentMethodServiceUnitTests
{
    [Fact]
    public async Task Should_FailGettingPaymentMethods_WhenUserIdIsEmpty()
    {
        await Assert.ThrowsAsync<ArgumentException>(
            () => _paymentMethodService.GetPaymentMethods(Guid.Empty)
        );
    }

    [Fact]
    public async Task Should_ReturnCopiesOfRequestedPaymentMethods()
    {
        var pm = _database.First();
        var result = await _paymentMethodService.GetPaymentMethods(pm.UserId);

        foreach (var entity in result)
        {
            Assert.NotSame(_database.Find(elem => elem.Id == entity.Id), entity);
        }
    }

    [Fact]
    public async Task Should_ReturnRequestedPaymentMethods()
    {
        var pm = _database.First();
        var result = await _paymentMethodService.GetPaymentMethods(pm.UserId);

        Assert.Equal(_database.Count(elem => elem.UserId == pm.UserId), result.Count());
    }
}
