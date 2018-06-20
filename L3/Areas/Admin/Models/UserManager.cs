using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace L3.Areas.Admin.Models
{
    public class L3UserManager: UserManager<User>
    {       
        public L3UserManager(IUserStore<User> store) : base(store) 
        {
        }
        public static L3UserManager Create(IdentityFactoryOptions<L3UserManager> options,IOwinContext context)
        {
            IdentityDB db = context.Get<IdentityDB>();
            L3UserManager manager = new L3UserManager(new UserStore<User>(db));
            return manager;
        }
        public async Task<bool> ChangePasswordAsync(L3UserManager userManager, User user, string newPassword)
        {
            var result = await userManager.RemovePasswordAsync(user.Id);
            if (result.Succeeded)
            {
                result = await userManager.AddPasswordAsync(user.Id, newPassword);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    //ModelState.AddModelError("", result.Errors.FirstOrDefault());
                }
            }
            else
            {
                //ModelState.AddModelError("", result.Errors.FirstOrDefault());
            }

            return false;
        }
    }
}