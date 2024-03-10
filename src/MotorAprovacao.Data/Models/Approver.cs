namespace MotorAprovacaoME.Data.Models;

public class Approver
{
    public Approver()
    {
        CreatedAt = DateTime.Now;
    }
    public Guid ApproverId { get; set; }
    public string ApproverName { get; set; }
    internal DateTime CreatedAt { get; set; }

}