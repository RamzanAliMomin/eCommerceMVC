using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UI.Security;

namespace UI.Areas.User.Controllers
{
    [CustomAuthorize(Roles ="User")]
    [Area("User")]
    public class BaseController : Controller
    {
    }
}