using BuildingBlocks.Domain.Enums;

namespace BuildingBlocks.Domain.Abstract;

public interface IEntity<TIdentifier>
{
    public TIdentifier Id { get; set; }
    public StatusType Status { get; set; }
    public DateTime CreateDate { get; set; }
    public TIdentifier CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public TIdentifier? UpdateUserId { get; set; }
    public DateTime? DeleteDate { get; set; }
    public TIdentifier? DeleteUserId { get; set; }
}