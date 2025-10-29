using BuildingBlocks.Services.Encryption;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BuildingBlocks.Services.CurrentUser;

public sealed class CurrentUser(IHttpContextAccessor httpContextAccessor, IEncryptionService encryptionService) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IEncryptionService _encryptionService = encryptionService;

    public long Id => Convert.ToInt64(FindClaim("Uid"));
    public string UserName => FindClaim(ClaimTypes.Name);
    public string Email => FindClaim(ClaimTypes.Email);

    private string FindClaim(string type)
    {
        try
        {
            ClaimsPrincipal User = _httpContextAccessor.HttpContext?.User;
            //Check user is authenticated
            if (User is null)
                return string.Empty;

            // Get the claim
            var claim = User.FindFirst(x => x.Type == type);
            if (claim == null)
                return string.Empty;

            return _encryptionService.Decrypt(claim.Value);
        }
        catch (Exception)
        {
            return null;
        }
    }
}
