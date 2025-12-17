namespace FinBookeAPI.Models.Payment;

public class PaymentMethod
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "";

    public IEnumerable<PaymentInstance> Instances { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

    public PaymentMethod() { }

    public PaymentMethod(PaymentMethod other)
    {
        Id = other.Id;
        Name = other.Name;
        UserId = other.UserId;
        CreatedAt = other.CreatedAt;
        ModifiedAt = other.ModifiedAt;
        foreach (var instance in other.Instances)
        {
            Instances = [.. Instances, new PaymentInstance(instance)];
        }
    }

    public override string ToString()
    {
        return $"PaymentMethod: {{ Id: {Id}, UserId: {UserId}, Name: {Name}, Instances: [{string.Join(", ", Instances)}], CreatedAt: {CreatedAt}, ModifiedAt: {ModifiedAt} }}";
    }
}
