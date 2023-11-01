namespace To_Do_List.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; } = DateTime.Now;
    public string? CreatedBy { get; set; }
    public DateTime Modified { get; set; } 
    public string? LastModifiedBy { get; set; }
}