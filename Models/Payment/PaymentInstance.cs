namespace FinBookeAPI.Models.Payment;

public class PaymentInstance
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public PaymentInstance() { }

    public PaymentInstance(PaymentInstance other)
    {
        Id = other.Id;
        Name = other.Name;
    }

    public override string ToString()
    {
        return $"PaymentInstance: {{ Id: {Id}, Name: {Name} }}";
    }
}
