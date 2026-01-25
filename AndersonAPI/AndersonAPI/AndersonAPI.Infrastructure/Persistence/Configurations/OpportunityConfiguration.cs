using AndersonAPI.Domain;
using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Persistence.Configurations
{
    public class OpportunityConfiguration : IEntityTypeConfiguration<Opportunity>
    {
        public void Configure(EntityTypeBuilder<Opportunity> builder)
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

            builder.Property(x => x.OpportunityTypeId)
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

            builder.HasOne(x => x.OpportunityType)
                .WithMany()
                .HasForeignKey(x => x.OpportunityTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.InterestedPartners)
                .WithMany(x => x.SavedOpportunities)
                .UsingEntity(x => x.ToTable("OpportunityCompanies"));

            builder.HasOne(x => x.Company)
                .WithMany(x => x.Opportunities)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(x => x.Messages, ConfigureMessages);

            builder.HasMany(x => x.ServiceTypes)
                .WithMany("Opportunities")
                .UsingEntity(x => x.ToTable("OpportunityServiceTypes"));

            builder.HasMany(x => x.Capabilities)
                .WithMany("Opportunities")
                .UsingEntity(x => x.ToTable("OpportunityCapabilities"));

            builder.HasMany(x => x.Industries)
                .WithMany("Opportunities")
                .UsingEntity(x => x.ToTable("OpportunityIndustries"));

            builder.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(tb => tb.HasCheckConstraint("opportunity_status_check", $"\"Status\" IN ({string.Join(",", Enum.GetValues<OpportunityStatus>().Select(e => $"'{e}'"))})"));

            builder.Ignore(e => e.DomainEvents);
        }

        [IntentManaged(Mode.Merge)]
        public static void ConfigureMessages(OwnedNavigationBuilder<Opportunity, Message> builder)
        {
            builder.WithOwner()
                .HasForeignKey(x => x.OpportunityId);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.OpportunityId)
                .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId)
                .IsRequired();

            builder.Property(x => x.CreatedByUser)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CreatedByPartner)
                .HasMaxLength(200);

            builder.Property(x => x.IsOwnMessage)
                .IsRequired();

            // IntentIgnore
            builder.Ignore(e => e.DomainEvents);
        }
    }
}