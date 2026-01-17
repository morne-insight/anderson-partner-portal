using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain;
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
            EntityState state)
        {
            Comment = comment;
            Rating = rating;
            ApplicationIdentityUserId = applicationIdentityUserId;
            ReviewerCompanyId = reviewerCompanyId;
            State = state;
        }

        public string Comment { get; set; }
        public int Rating { get; set; }
        public string ApplicationIdentityUserId { get; set; }
        public Guid ReviewerCompanyId { get; set; }
        public EntityState State { get; set; }
    }
}