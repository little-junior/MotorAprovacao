namespace MotorAprovacao.Models.Entities
{
    public class CategoryRules
    {
        public CategoryRules() { }
        public CategoryRules(int id, int categoryid, decimal maxApprove, decimal minDisapprove)
        {
            Id = id;
            CategoryId = categoryid;
            MaximumToApprove = maxApprove;
            MinimumToDisapprove = minDisapprove;
        }
        public int Id { get; set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        public decimal MaximumToApprove { get; private set; }
        public decimal MinimumToDisapprove { get; private set; }
    }
}