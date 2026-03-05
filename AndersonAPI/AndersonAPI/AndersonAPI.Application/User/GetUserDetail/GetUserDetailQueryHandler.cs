using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.AspNetCore.Identity;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.User.GetUserDetail
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetailDto>
    {
        private readonly IUserStore<ApplicationIdentityUser> _userStore;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager; 
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationIdentityUserRepository _applicationIdentityUserRepository;
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public GetUserDetailQueryHandler(
            IUserStore<ApplicationIdentityUser> userStore,
            UserManager<ApplicationIdentityUser> userManager,   
            RoleManager<IdentityRole<string>> roleManager,
            ICurrentUserService currentUserService,
            IApplicationIdentityUserRepository applicationIdentityUserRepository,
            ICompanyRepository companyRepository)
        {
            _userStore = userStore;
            _userManager = userManager;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _applicationIdentityUserRepository = applicationIdentityUserRepository;
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<UserDetailDto> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetAsync();
            if (user == null || user.Id == null) throw new UnauthorizedAccessException("The user is not authenticated");

            var applicationUser = await _applicationIdentityUserRepository
                .FindByIdAsync(user.Id.Value.ToString(), cancellationToken);

            if (applicationUser == null) throw new UnauthorizedAccessException("The user identity could not be found.");

            var companies = await _companyRepository
                .FindAllAsync(c => c.ApplicationIdentityUsers
                    .Any(u => u.Id == applicationUser.Id.ToString()), cancellationToken);

            var roles = await _userManager.GetRolesAsync(applicationUser);

            if (companies.Count == 0)
            {
                return UserDetailDto.Create(
                    name: applicationUser.Name ?? string.Empty,
                    companyId: null,
                    companyName: null,
                    companies: new List<UserCompanyDto>(),
                    roles: roles.ToList());
            }

            return UserDetailDto.Create(
                name: applicationUser.Name ?? string.Empty,
                companyId: companies[0].Id,
                companyName: companies[0].Name,
                companies: companies.Select(c => UserCompanyDto.Create(
                    id: c.Id,
                    name: c.Name,
                    websiteUrl: c.WebsiteUrl)).ToList(),
                roles: roles.ToList()
                );
        }
    }
}