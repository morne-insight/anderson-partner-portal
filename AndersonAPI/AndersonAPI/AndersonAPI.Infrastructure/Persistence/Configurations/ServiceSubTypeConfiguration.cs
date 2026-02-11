using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Persistence.Configurations
{
    public class ServiceSubTypeConfiguration : IEntityTypeConfiguration<ServiceSubType>
    {
        public void Configure(EntityTypeBuilder<ServiceSubType> builder)
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

            builder.Property(x => x.ServiceTypeId)
                .IsRequired();

            builder.HasOne(x => x.ServiceType)
                .WithMany()
                .HasForeignKey(x => x.ServiceTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(e => e.DomainEvents);
        }
    }
}