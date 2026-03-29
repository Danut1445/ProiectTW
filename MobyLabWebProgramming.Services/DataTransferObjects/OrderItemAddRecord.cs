namespace MobyLabWebProgramming.Services.DataTransferObjects;

public class OrderItemAddRecord
{
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
}