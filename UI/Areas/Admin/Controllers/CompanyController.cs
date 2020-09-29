using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DomainModels.Entities;
using BAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using DomainModels.Models;

namespace UI.Areas.Admin.Controllers
{
    public class CompanyController : BaseController
    {
        IEnumerable<Country> countries = new List<Country>();
        IEnumerable<State> states = new List<State>();
        IEnumerable<City> cities = new List<City>();

        public CompanyController(IUnitOfWork _uow) : base(_uow)
        {
        }

        public IActionResult Index()
        {
            return View(uow.CompanyRepo.GetAll());
        }
        void BindCountry()
        {
            countries = uow.CountryRepo.GetCountry();
            ViewBag.CountryList = countries;
        }
        public JsonResult GetState(int cid)
        {
            states = uow.StateRepo.GetState(cid);
            return Json(new SelectList(states, "StateId", "StateName"));
        }
        public JsonResult GetCity(int sid)
        {
            cities = uow.CityRepo.GetCity(sid);
            return Json(new SelectList(cities, "CityId", "CityName"));
        }

        public ActionResult Create()
        {
            BindCountry();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyModel  model, IFormCollection formcollection)
        {
            if (model.CountryId == 0)
            {
                ModelState.AddModelError("", "Please Select Country");
            }
            if (model.StateId == 0)
            {
                ModelState.AddModelError("", "Please Select State");
            }
            if (model.CityId == 0)
            {
                ModelState.AddModelError("", "Please Select City");
            }

            try
            {
                var CountryId = HttpContext.Request.Form["CountryId"].ToString();
                var StateId = HttpContext.Request.Form["StateId"].ToString();
                var CityId = HttpContext.Request.Form["Cityid"].ToString();

                model.CountryId = Convert.ToInt32(CountryId);
                model.StateId = Convert.ToInt32(StateId);
                model.CityId = Convert.ToInt32(CityId);

                Company company = new Company {
                    CompanyId = model.CompanyId,
                    CompanyName = model.CompanyName,
                    MobileNumber = model.MobileNumber,
                    DirectorName = model.DirectorName,
                    TypeOfCompany = model.TypeOfCompany,
                    CompanyStartDate = model.CompanyStartDate,
                    Level = model.Level,
                    TownName = model.TownName,
                    EmailAddress = model.EmailAddress,
                    CompanyLicenceDate = model.CompanyLicenceDate,
                    GrnNo = model.GrnNo,
                    CountryId = model.CountryId,
                    Address = model.Address,
                    StateId = model.StateId,
                    CityId = model.CityId,
                    PinCode = model.PinCode

                };
                uow.CompanyRepo.Add(company);
                uow.SaveChanges();
                BindCountry();
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}