using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(160);

            builder.Property(x => x.ShortDescription)
                .IsRequired();

            builder.Property(x => x.FullDescription)
                .IsRequired();

            builder.Property(x => x.WebsiteUrl)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.EmployeeCount)
                .IsRequired();

            builder.Property(x => x.Embedding);

            builder.Property(x => x.EmbeddingModel)
                .HasMaxLength(64);

            builder.Property(x => x.EmbeddingDim);

            builder.Property(x => x.EmbeddingUpdated);

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.UpdatedBy);

            builder.Property(x => x.UpdatedDate);

            builder.OwnsMany(x => x.Locations, ConfigureLocations);

            builder.HasMany(x => x.Industries)
                .WithMany("Company")
                .UsingEntity(x => x.ToTable("CompanyIndustries"));

            builder.HasMany(x => x.Capabilities)
                .WithMany("Company")
                .UsingEntity(x => x.ToTable("CompanyCapabilities"));

            builder.HasMany(x => x.ServiceTypes)
                .WithMany("Company")
                .UsingEntity(x => x.ToTable("CompanyServiceTypes"));

            builder.OwnsMany(x => x.Contacts, ConfigureContacts);

            builder.HasMany(x => x.ApplicationIdentityUsers)
                .WithMany("Company")
                .UsingEntity(x => x.ToTable("CompanyApplicationIdentityUsers"));

            builder.Ignore(e => e.DomainEvents);
        }

        [IntentManaged(Mode.Merge)]
        public static void ConfigureLocations(OwnedNavigationBuilder<Company, Location> builder)
        {
            builder.WithOwner()
                .HasForeignKey(x => x.CompanyId);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.IsHeadOffice)
                .IsRequired();

            builder.Property(x => x.RegionId)
                .IsRequired();

            builder.Property(x => x.CountryId)
                .IsRequired();

            builder.Property(x => x.CompanyId)
                .IsRequired();

            builder.HasOne(x => x.Region)
                .WithMany()
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(e => e.DomainEvents);
        }

        [IntentManaged(Mode.Merge)]
        public static void ConfigureContacts(OwnedNavigationBuilder<Company, Contact> builder)
        {
            builder.WithOwner()
                .HasForeignKey(x => x.CompanyId);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.FirstName)
                .IsRequired();

            builder.Property(x => x.LastName)
                .IsRequired();

            builder.Property(x => x.EmailAddress);

            builder.Property(x => x.CompanyPosition);

            builder.Property(x => x.CompanyId)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.UpdatedBy);

            builder.Property(x => x.UpdatedDate);

            builder.Ignore(e => e.DomainEvents);
        }
    }
}