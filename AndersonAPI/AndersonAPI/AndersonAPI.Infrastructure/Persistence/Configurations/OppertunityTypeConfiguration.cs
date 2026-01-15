using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Persistence.Configurations
{
    public class OppertunityTypeConfiguration : IEntityTypeConfiguration<OppertunityType>
    {
        public void Configure(EntityTypeBuilder<OppertunityType> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Ignore(e => e.DomainEvents);
        }
    }
}