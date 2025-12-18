namespace FinBookeAPI.Models.Payment;

public class PaymentInstance
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Details { get; set; } = "";

    public PaymentInstance() { }

    public PaymentInstance(PaymentInstance other)
    {
        Id = other.Id;
        Details = new string(other.Details);
    }

    public override string ToString()
    {
        return $"PaymentInstance: {{ Id: {Id}, Details: {Details} }}";
    }
}
