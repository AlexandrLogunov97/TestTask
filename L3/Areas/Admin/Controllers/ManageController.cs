using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.Routing;
using L3.Areas.Admin.Models;
using L3.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;

namespace L3.Areas.Admin.Controllers
{

    [AdminAuth]
    public class ManageController : Controller
    {
        private L3UserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<L3UserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public async Task<ActionResult> Edit(string id)
        {

            User user = await UserManager.FindByIdAsync(id);
            
            if (user != null)
            {
                EditModel editModel=new EditModel(){UserId = user.Id,UserName = user.UserName,Role = UserManager.GetRoles(user.Id).FirstOrDefault()};
                if(editModel.Role!="Admin" && UserManager.Users.Count()>1)
                    return View(editModel);
            }

            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditModel model,string oldRole)
        {
            User user = await UserManager.FindByIdAsync(model.UserId);
            if (user != null)
            {
                
                user.UserName = model.UserName;
                if (oldRole != model.Role)
                {
                    await UserManager.RemoveFromRoleAsync(user.Id, oldRole);
                    await UserManager.AddToRoleAsync(user.Id, model.Role);
                }

                if (!string.IsNullOrEmpty(model.Password))
                {
                    if (!await UserManager.ChangePasswordAsync(UserManager, user, model.Password))
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }

                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {

                    return RedirectToAction("Users");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }
        public ActionResult Users()
        {
            var users = UserManager.Users;
            List<UserRole> usersRoles=new List<UserRole>();
            foreach (var x in users)
            {
                usersRoles.Add(new UserRole(){User = x, Role = UserManager.GetRoles(x.Id).FirstOrDefault()});
            }
            return View(usersRoles);
        }
        public ActionResult Create()
        {
            return View(new CreateModel());
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.Login };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);
                    return RedirectToAction("Users", "Manage");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            if(user!=null)
                return View(user);
            return RedirectToAction("Users", "Manage");
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                if (UserManager.GetRoles(user.Id).FirstOrDefault() != "Admin" && UserManager.Users.Count() > 1)
                {
                    IdentityResult result = await UserManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Users", "Manage");
                    }
                }
            }
            return RedirectToAction("Users", "Manage");
        }
    }
}