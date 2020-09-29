using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL;
using Microsoft.AspNetCore.Mvc;
using UI.Security;

namespace UI.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles ="Admin")]
    [Area("Admin")]
    public class BaseController : Controller
    {
        protected IUnitOfWork uow;
        public BaseController(IUnitOfWork _uow)
        {
            uow = _uow;
        } 
    }
}