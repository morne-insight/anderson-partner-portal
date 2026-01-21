using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Invites.GetInvites
{
    public class GetInvitesQuery : IRequest<List<InviteDto>>, IQuery
    {
        public GetInvitesQuery()
        {
        }
    }
}