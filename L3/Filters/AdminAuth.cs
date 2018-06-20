using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace L3.Filters
{
    public class AdminAuthAttribute : FilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated || !user.IsInRole("Admin"))
            {
                filterContext.Result = new RedirectResult("~/Cars/Index");
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated || !user.IsInRole("Admin"))
            {
                filterContext.Result = new RedirectResult("~/Cars/Index");
            }
        }
    }
}