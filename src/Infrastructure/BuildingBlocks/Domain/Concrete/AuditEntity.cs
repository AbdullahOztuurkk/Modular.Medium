using BuildingBlocks.Domain.Abstract;
using BuildingBlocks.Domain.Enums;

namespace BuildingBlocks.Domain.Common;

public class AuditEntity : IEntity
{
    public long Id { get; set; }
    public StatusType Status { get; set; } = StatusType.Active;
    public DateTime CreateDate { get; set; }
    public long CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public long? UpdateUserId { get; set; }
    public DateTime? DeleteDate { get; set; }
    public long? DeleteUserId { get; set; }
}