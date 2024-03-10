namespace MotorAprovacaoME.Data;

public class CategoryLegate
{
    public CategoryLegate()
    {
        CreatedAt = DateTime.Now;
    }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    internal DateTime CreatedAt { get; set; }
}