namespace FinBookeAPI.Tests.Payment;

public partial class PaymentMethodServiceUnitTests
{
    [Fact]
    public async Task Should_ReturnCopiesOfRequestedPaymentMethods()
    {
        var pm = _database.First();
        var result = await _paymentMethodService.GetPaymentMethods(pm.UserId);

        foreach (var entity in result)
        {
            var dbpm = _database.Find(elem => elem.Id == entity.Id);
            Assert.NotSame(dbpm, entity);
            Assert.NotSame(dbpm!.Type, entity.Type);
            foreach (var instance in entity.Instances)
            {
                var dbpi = dbpm.Instances.First(elem => elem.Id == instance.Id);
                Assert.NotSame(dbpi, instance);
                Assert.NotSame(dbpi.Name, instance.Name);
                if (dbpi.Description is not null)
                    Assert.NotSame(dbpi.Description, instance.Description);
            }
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
