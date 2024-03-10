namespace MotorAprovacao.Models.Entities
{
    public class CategoryRules
    {
        public CategoryRules() { }
        public int Id { get; set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public decimal MaximumToApprove { get; private set; }
        public decimal MinimumToDisapprove { get; private set; }
    }
}