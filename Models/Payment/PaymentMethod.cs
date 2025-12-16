namespace FinBookeAPI.Models.Payment;

public class PaymentMethod
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public IEnumerable<PaymentInstance> Instances { get; set; } = [];
}
