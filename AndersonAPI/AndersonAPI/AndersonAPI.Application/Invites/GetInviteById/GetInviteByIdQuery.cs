using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Invites.GetInviteById
{
    public class GetInviteByIdQuery : IRequest<InviteDto>, IQuery
    {
        public GetInviteByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}