using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Reviews.UpdateReview
{
    public class UpdateReviewCommand : IRequest, ICommand
    {
        public UpdateReviewCommand(Guid id, string comment, int rating)
        {
            Id = id;
            Comment = comment;
            Rating = rating;
        }

        public Guid Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}