using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Reviews.CreateReview
{
    public class CreateReviewCommand : IRequest<Guid>, ICommand
    {
        public CreateReviewCommand(string comment,
            int rating,
            string applicationIdentityUserId,
            Guid reviewerCompanyId,
            Guid companyId)
        {
            Comment = comment;
            Rating = rating;
            ApplicationIdentityUserId = applicationIdentityUserId;
            ReviewerCompanyId = reviewerCompanyId;
            CompanyId = companyId;
        }

        public string Comment { get; set; }
        public int Rating { get; set; }
        public string ApplicationIdentityUserId { get; set; }
        public Guid ReviewerCompanyId { get; set; }
        public Guid CompanyId { get; set; }
    }
}