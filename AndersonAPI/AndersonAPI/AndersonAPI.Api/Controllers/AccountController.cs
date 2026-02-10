using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using AndersonAPI.Api.Services;
using AndersonAPI.Application.Account;
using AndersonAPI.Application.Capabilities.DeleteCapability;
using AndersonAPI.Application.Invites.AcceptInvite;
using AndersonAPI.Application.Invites.GetInvitesByUserId;
using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.JsonWebTokens;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.AspNetCore.Identity.AccountController.AccountController", Version = "1.0")]

namespace AndersonAPI.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [IntentManaged(Mode.Merge)]
    public class AccountController : ControllerBase
    {
        // Validate the email address using DataAnnotations like the UserValidator does when RequireUniqueEmail = true.
        private static readonly EmailAddressAttribute EmailAddressAttribute = new EmailAddressAttribute();
        private readonly IUserStore<ApplicationIdentityUser> _userStore;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole<string>> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountEmailSender _accountEmailSender;
        private readonly ITokenService _tokenService;
        private readonly ISender _mediator;

        [IntentManaged(Mode.Merge)]
        public AccountController(IUserStore<ApplicationIdentityUser> userStore,
                    UserManager<ApplicationIdentityUser> userManager,
                    RoleManager<IdentityRole<string>> roleManager,
                    ILogger<AccountController> logger,
                    IAccountEmailSender accountEmailSender,
                    ITokenService tokenService,
                    ISender mediator)
        {
            _userStore = userStore;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _accountEmailSender = accountEmailSender;
            _tokenService = tokenService;
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [AllowAnonymous]
        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public async Task<IActionResult> Register(RegisterDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
            {
                ModelState.AddModelError<RegisterDto>(x => x.Email, "Mandatory");
            }

            if (string.IsNullOrWhiteSpace(input.Password))
            {
                ModelState.AddModelError<RegisterDto>(x => x.Password, "Mandatory");
            }

            if (string.IsNullOrWhiteSpace(input.UserName))
            {
                ModelState.AddModelError<RegisterDto>(x => x.UserName, "Mandatory");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationIdentityUser
            {
                Id = Guid.NewGuid().ToString()
            };

            user.SetName(input.UserName);

            await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
            await _userManager.SetEmailAsync(user, input.Email);
            var result = await _userManager.CreateAsync(user, input.Password!);

            if (!result.Succeeded)
            {
                var errorMessage = "";
                foreach (var error in result.Errors)
                {
                    if (error.Code == "PasswordRequiresNonAlphanumeric")
                    {
                        ModelState.AddModelError("errors", "Password must contain at least one non-alphanumeric character. (!@#$%^&*).");
                    }
                    else 
                    { 
                        ModelState.AddModelError("errors", error.Description);
                    }
                }

                return BadRequest(ModelState);
            }

            _logger.LogInformation("User created a new account with password.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                await SendConfirmationEmail(user);
            }

            return Ok();
        }

        [HttpPost]
        [Authorize]
        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public async Task<IActionResult> RegisterAdmin(RegisterDto input)
        {
            var registerResult = await Register(input);
            if (registerResult is not OkResult)
            {
                return registerResult;
            }
            var user = await _userManager.FindByEmailAsync(input.Email!);
            if (user == null)
            {
                return NotFound($"User with email '{input.Email}' not found after registration.");
            }
            var roleExists = await _roleManager.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<string> { Name = "Admin", Id = Guid.NewGuid().ToString() });
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, $"Failed to create 'Admin' role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
            var addToRoleResult = await _userManager.AddToRoleAsync(user, "Admin");
            if (!addToRoleResult.Succeeded)
            {
                return StatusCode(500, $"Failed to add user to 'Admin' role: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
            }
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public async Task<IActionResult> ResendConfirmationEmail(ResendEmailDto input) 
        {
            if (string.IsNullOrEmpty(input.Email))
            {
                ModelState.AddModelError<ResendEmailDto>(x => x.Email, "Mandatory");
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(input.Email!);
            if (user == null)
            {
                ModelState.AddModelError("errors", "User is not registered");
                return BadRequest();
            }

            _logger.LogInformation("User found, sending email.");

            await SendConfirmationEmail(user);

            return Ok();

        }

        [HttpPost]
        [AllowAnonymous]
        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public async Task<IActionResult> RegisterWithRole(RegisterWithRoleDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
            {
                ModelState.AddModelError<RegisterWithRoleDto>(x => x.Email, "Mandatory");
            }

            if (string.IsNullOrWhiteSpace(input.Password))
            {
                ModelState.AddModelError<RegisterWithRoleDto>(x => x.Password, "Mandatory");
            }

            if (string.IsNullOrWhiteSpace(input.UserName))
            {
                ModelState.AddModelError<RegisterWithRoleDto>(x => x.UserName, "Mandatory");
            }

            if (string.IsNullOrWhiteSpace(input.Role))
            {
                ModelState.AddModelError<RegisterWithRoleDto>(x => x.Role, "Mandatory");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roleExists = await _roleManager.RoleExistsAsync(input.Role!);
            if (!roleExists)
            {
                ModelState.AddModelError<RegisterWithRoleDto>(x => x.Role, "Role does not exist.");
                return BadRequest(ModelState);
            }

            var user = new ApplicationIdentityUser { Id = Guid.NewGuid().ToString() };
            user.SetName(input.UserName);

            await _userStore.SetUserNameAsync(user, input.Email, CancellationToken.None);
            await _userManager.SetEmailAsync(user, input.Email);
            var result = await _userManager.CreateAsync(user, input.Password!);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, input.Role!);

            _logger.LogInformation("User created a new account with password.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                await SendConfirmationEmail(user);
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public async Task<ActionResult> CreateRole(RoleDto role)
        {
            if (string.IsNullOrEmpty(role.Name))
            {
                return BadRequest("Role is required.");
            }

            if (_roleManager == null)
            {
                return StatusCode(500, "RoleManager service not available.");
            }

            if (await _roleManager.RoleExistsAsync(role.Name))
            {
                return Conflict($"Role '{role.Name}' already exists.");
            }

            // Create the role
            var newRole = new IdentityRole<string>(role.Name);
            newRole.Id = Guid.NewGuid().ToString();
            var result = await _roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok($"Role '{newRole.Name}' created successfully.");
        }

        [HttpPost]
        [AllowAnonymous]
        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public async Task<ActionResult<TokenResultDto>> Login(LoginDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
            {
                ModelState.AddModelError<LoginDto>(x => x.Email, "Mandatory");
            }

            if (string.IsNullOrWhiteSpace(input.Password))
            {
                ModelState.AddModelError<LoginDto>(x => x.Password, "Mandatory");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var email = input.Email!;
            var password = input.Password!;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || 
                !await _userManager.CheckPasswordAsync(user, password))
            {
                _logger.LogWarning("Invalid login attempt.");
                return Forbid();
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("Email not confirmed.");
                return Forbid();
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("User account locked out.");
                return Forbid();
            }

            var claims = await GetClaims(user);

            var (token, expiry) = _tokenService.GenerateAccessToken(username: user.Email!, claims: claims.ToArray());
            var (refreshToken, refreshTokenExpiry) = _tokenService.GenerateRefreshToken(user.Email!);

            user.UpdateRefreshToken(refreshToken, refreshTokenExpiry);

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("User logged in.");

            return Ok(new TokenResultDto
            {
                AuthenticationToken = token,
                ExpiresIn = (int)(expiry - DateTime.UtcNow).TotalSeconds,
                RefreshToken = refreshToken,
                UserName = user.Name,
                UserId = user.Id
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public async Task<ActionResult<TokenResultDto>> Refresh(RefreshTokenDto dto)
        {
            var username = _tokenService.GetUsernameFromRefreshToken(dto.RefreshToken);
            if (username == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != dto.RefreshToken)
            {
                return BadRequest();
            }

            var claims = await GetClaims(user);

            var (token, expiry) = _tokenService.GenerateAccessToken(user.Email!, claims);
            var (refreshToken, refreshTokenExpiry) = _tokenService.GenerateRefreshToken(user.Email!);

            user.UpdateRefreshToken(refreshToken, refreshTokenExpiry);

            await _userManager.UpdateAsync(user);

            return Ok(new TokenResultDto
            {
                AuthenticationToken = token,
                ExpiresIn = (int)(expiry - DateTime.UtcNow).TotalSeconds,
                RefreshToken = refreshToken
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto input)
        {
            if (string.IsNullOrWhiteSpace(input.UserId))
            {
                ModelState.AddModelError<ConfirmEmailDto>(x => x.UserId, "Mandatory");
            }

            if (string.IsNullOrWhiteSpace(input.Code))
            {
                ModelState.AddModelError<ConfirmEmailDto>(x => x.Code, "Mandatory");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = input.UserId!;
            var code = input.Code!;
            var user = await _userManager.FindByIdAsync(input.UserId!);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                ModelState.AddModelError<ConfirmEmailDto>(x => x, "Error confirming your email.");
                return BadRequest(ModelState);
            }

            var invites = await _mediator.Send(new GetInvitesByUserIdQuery(user.Id));

            foreach (var invite in invites)
            {
                await _mediator.Send(new AcceptInviteCommand(invite.Id, user.Id));
            }

            return Ok();
        }

        [HttpPost("~/api/[controller]/forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto resetRequest)
        {
            var user = await _userManager.FindByEmailAsync(resetRequest.Email!);

            if (user is not null && await _userManager.IsEmailConfirmedAsync(user))
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                await _accountEmailSender.SendPasswordResetCode(resetRequest.Email!, user.Id,
                    HtmlEncoder.Default.Encode(code));
            }

            // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
            // returned a 400 for an invalid code given a valid user email.
            return Ok();
        }

        [HttpPost("~/api/[controller]/resetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetRequest)
        {
            var modelState = new ModelStateDictionary();

            var user = await _userManager.FindByEmailAsync(resetRequest.Email!);

            if (user is null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                // Don't reveal that the user does not exist or is not confirmed, so don't return a 200 if we would have
                // returned a 400 for an invalid code given a valid user email.
                modelState.AddModelError<ResetPasswordDto>(x => x.ResetCode, "Invalid token");
                return ValidationProblem();
            }

            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode!));
                result = await _userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword!);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(string.Empty, error.Description);
                }

                return ValidationProblem(modelState);
            }

            return Ok();
        }

        [HttpGet("~/api/[controller]/manage/info")]
        [Authorize]
        public async Task<ActionResult<InfoResponseDto>> GetInfo()
        {
            var user = await _userManager.GetUserAsync(User);

            return new InfoResponseDto
            {
                Email = user?.Email
            };
        }

        [HttpPost("~/api/[controller]/manage/info")]
        [Authorize]
        public async Task<ActionResult<InfoResponseDto>> PostInfo(UpdateInfoDto infoRequest)
        {
            if (await _userManager.GetUserAsync(User) is not { } user)
            {
                return NotFound();
            }

            var modelState = new ModelStateDictionary();

            if (!string.IsNullOrEmpty(infoRequest.NewEmail) && !EmailAddressAttribute.IsValid(infoRequest.NewEmail))
            {
                modelState.AddModelError<UpdateInfoDto>(x => x.NewEmail, "Invalid email address.");
                return ValidationProblem(modelState);
            }

            if (!string.IsNullOrEmpty(infoRequest.NewPassword))
            {
                if (string.IsNullOrEmpty(infoRequest.OldPassword))
                {
                    modelState.AddModelError<UpdateInfoDto>(x => x.OldPassword, "The old password is required to set a new password. If the old password is forgotten, use /resetPassword.");
                    return ValidationProblem(modelState);
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        modelState.AddModelError<UpdateInfoDto>(x => x.NewPassword, error.Description);
                    }

                    return ValidationProblem(modelState);
                }
            }

            if (!string.IsNullOrEmpty(infoRequest.NewEmail))
            {
                var email = await _userManager.GetEmailAsync(user);
                if (email != infoRequest.NewEmail)
                {
                    await _userStore.SetUserNameAsync(user, infoRequest.NewEmail, CancellationToken.None);
                    await _userManager.SetEmailAsync(user, infoRequest.NewEmail);
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        await SendConfirmationEmail(user);
                    }
                }
            }

            return new InfoResponseDto
            {
                Email = user.Email
            };
        }

        [HttpPost]
        [Authorize]
        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity!.Name!;
            var user = (await _userManager.FindByNameAsync(username))!;

            if (user == null)
            {
                user = (await _userManager.FindByIdAsync(username))!;
            }

            user.UpdateRefreshToken(null, null);

            await _userManager.UpdateAsync(user);

            _logger.LogInformation($"User [{username}] logged out the system.");
            return Ok();
        }

        private async Task SendConfirmationEmail(ApplicationIdentityUser user)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var userId = await _userManager.GetUserIdAsync(user);

            await _accountEmailSender.SendEmailConfirmationRequest(
                email: user.Email!,
                userId: userId,
                code: code);
        }

        private async Task<IList<Claim>> GetClaims(ApplicationIdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            return claims;
        }
    }

    [IntentManaged(Mode.Merge)]
    public class TokenResultDto
    {
        public string TokenType => "Bearer";
        public string? AuthenticationToken { get; set; }
        public int ExpiresIn { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
    }

    [IntentManaged(Mode.Merge)]
    public class RegisterDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }

    }

    [IntentManaged(Mode.Ignore)]
    public class ResendEmailDto
    {
        public string? Email { get; set; }
    }

    [IntentManaged(Mode.Ignore)]
    public class RegisterWithRoleDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }

    [IntentManaged(Mode.Ignore)]
    public class RoleDto
    {
        public string? Name { get; set; }
    }

    public class LoginDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class ConfirmEmailDto
    {
        public string? UserId { get; set; }
        public string? Code { get; set; }
    }

    public class RefreshTokenDto
    {
        public string? RefreshToken { get; set; }
    }

    public class UpdateInfoDto
    {
        public string? NewEmail { get; set; }
        public string? NewPassword { get; set; }
        public string? OldPassword { get; set; }
    }

    public class InfoResponseDto
    {
        public string? Email { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string? Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public string? Email { get; set; }
        public string? ResetCode { get; set; }
        public string? NewPassword { get; set; }
    }
}