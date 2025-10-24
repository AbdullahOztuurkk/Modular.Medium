using BuildingBlocks.Domain.Abstract;
using BuildingBlocks.Domain.Enums;

namespace BuildingBlocks.Domain.Common;

public class AuditEntity<TIdentifier> : IEntity<TIdentifier>
{
    public TIdentifier Id { get; set; }
    public StatusType Status { get; set; }
    public DateTime CreateDate { get; set; }
    public TIdentifier CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public TIdentifier UpdateUserId { get; set; }
    public DateTime? DeleteDate { get; set; }
    public TIdentifier? DeleteUserId { get; set; }
}

public class AuditEntity : AuditEntity<long>
{

}