using Microsoft.AspNetCore.Identity;
using Sion.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sion.BLL.Services
{

    public class UsuarioService : IUsuarioService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsuarioService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string?> GetEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email;
        }

        public async Task<bool> EsAdminAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}
