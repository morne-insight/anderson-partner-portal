using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.EntityTypeConfiguration", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Order)
                .IsRequired();

            builder.Property(x => x.Comment)
                .IsRequired();

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedDate)
                .IsRequired();

            builder.Property(x => x.UpdatedBy);

            builder.Property(x => x.UpdatedDate);

            builder.Property(x => x.ApplicationIdentityUserId)
                .IsRequired();

            builder.Property(x => x.ReviewerCompanyProfileId)
                .IsRequired();

            builder.HasMany(x => x.CompanyProfiles)
                .WithMany(x => x.Reviews)
                .UsingEntity(x => x.ToTable("ReviewCompanyProfiles"));

            builder.HasOne(x => x.ReviewerCompanyProfile)
                .WithMany()
                .HasForeignKey(x => x.ReviewerCompanyProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApplicationIdentityUser)
                .WithMany()
                .HasForeignKey(x => x.ApplicationIdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(e => e.DomainEvents);
        }
    }
}