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
            Claim? claim = User.Claims.Where(c => c.Type == "id").FirstOrDefault();
            return claim?.Value;
        }

        private protected bool isAdmin()
        {
            Claim? claim = User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault();
            return claim != null && claim.Value == Roles.Admin.getDescription();
        }
    }
}
