namespace Whofax.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
