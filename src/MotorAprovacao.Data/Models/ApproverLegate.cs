namespace MotorAprovacaoME.Data;

public class ApproverLegate
{
    public ApproverLegate()
    {
        CreatedAt = DateTime.Now;
    }
    public Guid ApproverId { get; set; }
    public string ApproverName { get; set; }
    internal DateTime CreatedAt { get; set; }

}