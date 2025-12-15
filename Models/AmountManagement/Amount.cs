namespace FinBookeAPI.Models.AmountManagement;

public class Amount
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; } = Guid.NewGuid();

    public Guid PaymentTypeId { get; set; } = Guid.NewGuid();

    public Guid CategoryId { get; set; } = Guid.NewGuid();

    public decimal Value { get; set; }

    public AmountType Type { get; set; }

    public string Comment { get; set; } = "";

    public string ReceiptPath { get; set; } = "";

    public string BankStatementPath { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}
