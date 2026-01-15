using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Persistence.Configurations
{
    public class CompanyProfileConfiguration : IEntityTypeConfiguration<CompanyProfile>
    {
        public void Configure(EntityTypeBuilder<CompanyProfile> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
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

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.UpdatedBy);

            builder.Property(x => x.UpdatedDate);

            builder.OwnsMany(x => x.Locations, ConfigureLocations);

            builder.HasMany(x => x.Industries)
                .WithMany("CompanyProfiles")
                .UsingEntity(x => x.ToTable("CompanyProfileIndustries"));

            builder.HasMany(x => x.Capabilities)
                .WithMany("CompanyProfiles")
                .UsingEntity(x => x.ToTable("CompanyProfileCapabilities"));

            builder.HasMany(x => x.ServiceTypes)
                .WithMany("CompanyProfiles")
                .UsingEntity(x => x.ToTable("CompanyProfileServiceTypes"));

            builder.OwnsMany(x => x.Contacts, ConfigureContacts);

            builder.HasMany(x => x.ApplicationIdentityUsers)
                .WithMany("CompanyProfiles")
                .UsingEntity(x => x.ToTable("CompanyProfileApplicationIdentityUsers"));

            builder.Ignore(e => e.DomainEvents);
        }

        public static void ConfigureLocations(OwnedNavigationBuilder<CompanyProfile, Location> builder)
        {
            builder.WithOwner()
                .HasForeignKey(x => x.CompanyProfileId);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
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

            builder.Property(x => x.CompanyProfileId)
                .IsRequired();

            builder.HasOne(x => x.Region)
                .WithMany()
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void ConfigureContacts(OwnedNavigationBuilder<CompanyProfile, Contact> builder)
        {
            builder.WithOwner()
                .HasForeignKey(x => x.CompanyProfileId);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.FirstName)
                .IsRequired();

            builder.Property(x => x.LastName)
                .IsRequired();

            builder.Property(x => x.EmailAddress);

            builder.Property(x => x.CompanyPosition);

            builder.Property(x => x.CompanyProfileId)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.UpdatedBy);

            builder.Property(x => x.UpdatedDate);
        }
    }
}