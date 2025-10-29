using BuildingBlocks.Domain.Abstract;
using BuildingBlocks.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BuildingBlocks.Database.EntityFrameworkCore.Configurations;

public class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Status).IsRequired().HasMaxLength(20);
        builder.Property(x => x.CreateDate).IsRequired();
        builder.Property(x => x.UpdateDate).IsRequired(false);
        builder.Property(x => x.DeleteDate).IsRequired(false);

        builder.HasQueryFilter(x => x.Status == StatusType.Active);
        builder.HasIndex(x => x.Status);
    }
}
