using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.UpdateOppertunity
{
    public class UpdateOppertunityCommand : IRequest, ICommand
    {
        public UpdateOppertunityCommand(Guid id,
            string title,
            string shortDescription,
            string fullDescription,
            DateOnly? deadline,
            Guid oppertunityTypeId,
            Guid countryId)
        {
            Id = id;
            Title = title;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            Deadline = deadline;
            OppertunityTypeId = oppertunityTypeId;
            CountryId = countryId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public DateOnly? Deadline { get; set; }
        public Guid OppertunityTypeId { get; set; }
        public Guid CountryId { get; set; }
    }
}