namespace MotorAprovacaoME.Data.Models;

public class Category
{
    public Category()
    {
        CreatedAt = DateTime.Now;
    }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    internal DateTime CreatedAt { get; set; }
}