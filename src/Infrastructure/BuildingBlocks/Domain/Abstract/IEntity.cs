using BuildingBlocks.Domain.Enums;

namespace BuildingBlocks.Domain.Abstract;

public interface IEntity
{
    public long Id { get; set; }
    public StatusType Status { get; set; }
    public DateTime CreateDate { get; set; }
    public long CreateUserId { get; set; }
    public DateTime? UpdateDate { get; set; }
    public long? UpdateUserId { get; set; }
    public DateTime? DeleteDate { get; set; }
    public long? DeleteUserId { get; set; }
}