using AndersonAPI.Domain;
using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Persistence.Configurations
{
    public class QuarterlyConfiguration : IEntityTypeConfiguration<Quarterly>
    {
        public void Configure(EntityTypeBuilder<Quarterly> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.Year)
                .IsRequired();

            builder.Property(x => x.Quarter)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.UpdatedBy);

            builder.Property(x => x.UpdatedDate);

            builder.Property(x => x.IsSubmitted)
                .IsRequired();

            builder.Property(x => x.SubmittedDate)
                .IsRequired();

            builder.Property(x => x.CompanyId)
                .IsRequired();

            builder.HasOne(x => x.Company)
                .WithMany(x => x.Quarterlies)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.OwnsMany(x => x.Partners, ConfigurePartners);

            builder.OwnsMany(x => x.ReportLines, ConfigureReportLines);

            builder.ToTable(tb => tb.HasCheckConstraint("quarterly_quarter_check", $"\"Quarter\" IN ({string.Join(",", Enum.GetValues<ReportQuarter>().Select(e => $"'{e}'"))})"));

            builder.Ignore(e => e.DomainEvents);
        }

        [IntentManaged(Mode.Merge)]
        public static void ConfigurePartners(OwnedNavigationBuilder<Quarterly, ReportPartner> builder)
        {
            builder.WithOwner()
                .HasForeignKey(x => x.QuaterlyId);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired();

            builder.Property(x => x.QuaterlyId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.ToTable(tb => tb.HasCheckConstraint("report_partner_status_check", $"\"Status\" IN ({string.Join(",", Enum.GetValues<PartnerStatus>().Select(e => $"'{e}'"))})"));

            builder.Ignore(e => e.DomainEvents);
        }
        [IntentManaged(Mode.Merge)]
        public static void ConfigureReportLines(OwnedNavigationBuilder<Quarterly, ReportLine> builder)
        {
            builder.WithOwner()
                .HasForeignKey(x => x.QuarterlyId);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.State)
                .IsRequired();

            builder.Property(x => x.PartnerCount)
                .IsRequired();

            builder.Property(x => x.Headcount)
                .IsRequired();

            builder.Property(x => x.ClientCount)
                .IsRequired();

            builder.Property(x => x.OfficeCount)
                .IsRequired();

            builder.Property(x => x.LawyerCount)
                .IsRequired();

            builder.Property(x => x.EstimatedRevenue)
                .IsRequired();

            builder.Property(x => x.QuarterlyId)
                .IsRequired();

            builder.Property(x => x.CountryId)
                .IsRequired();

            builder.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(e => e.DomainEvents);
        }
    }
}