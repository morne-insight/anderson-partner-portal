using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.User.GetUserDetail
{
    public class GetUserDetailQuery : IRequest<UserDetailDto>, IQuery
    {
        public GetUserDetailQuery()
        {
        }
    }
}