namespace BuildingBlocks.Domain.Abstract;

public interface IConvertibleFrom<in TSource, out TDestination>
    where TSource : new()
    where TDestination : new()
{
    TDestination Map(TSource entity);
}