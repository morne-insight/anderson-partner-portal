using AndersonAPI.Domain;
using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Persistence.Configurations
{
    public class OppertunityConfiguration : IEntityTypeConfiguration<Oppertunity>
    {
        public void Configure(EntityTypeBuilder<Oppertunity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.Title)
                .IsRequired();

            builder.Property(x => x.ShortDescription)
                .IsRequired();

            builder.Property(x => x.FullDescription)
                .IsRequired();

            builder.Property(x => x.Deadline);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.OppertunityTypeId)
                .IsRequired();

            builder.Property(x => x.CountryId)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.UpdatedBy);

            builder.Property(x => x.UpdatedDate);

            builder.Property(x => x.CompanyId)
                .IsRequired();

            builder.HasOne(x => x.OppertunityType)
                .WithMany()
                .HasForeignKey(x => x.OppertunityTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ServiceTypes)
                .WithMany("Oppertunities")
                .UsingEntity(x => x.ToTable("OppertunityServiceTypes"));

            builder.HasMany(x => x.Capabilities)
                .WithMany("Oppertunities")
                .UsingEntity(x => x.ToTable("OppertunityCapabilities"));

            builder.HasMany(x => x.Industries)
                .WithMany("Oppertunities")
                .UsingEntity(x => x.ToTable("OppertunityIndustries"));

            builder.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.InterestedPartners)
                .WithMany(x => x.SavedOppertunities)
                .UsingEntity(x => x.ToTable("OppertunityCompanies"));

            builder.HasOne(x => x.Company)
                .WithMany(x => x.Oppertunities)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(x => x.Messages, ConfigureMessages);

            builder.ToTable(tb => tb.HasCheckConstraint("oppertunity_status_check", $"\"Status\" IN ({string.Join(",", Enum.GetValues<OppertunityStatus>().Select(e => $"'{e}'"))})"));

            builder.Ignore(e => e.DomainEvents);
        }

        [IntentManaged(Mode.Merge)]
        public static void ConfigureMessages(OwnedNavigationBuilder<Oppertunity, Message> builder)
        {
            builder.WithOwner()
                .HasForeignKey(x => x.OppertunityId);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.OppertunityId)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.CreatedByUser)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CreatedByPartner)
                .HasMaxLength(200);

            builder.Ignore(e => e.DomainEvents);
        }
    }
}