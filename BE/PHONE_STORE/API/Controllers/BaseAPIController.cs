using Common.Enums;
using DataAccess.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public class BaseAPIController : ControllerBase
    {
        private protected string? getUserId()
        {
            Claim? claim = User.Claims.FirstOrDefault(c => c.Type == "id");
            return claim?.Value;
        }

        private protected bool isAdmin()
        {
            Claim? claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return claim != null && claim.Value == Roles.Admin.getDescription();
        }
    }
}
