using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DomainModels.Entities;
using BAL;
using System.Data;
using System.Reflection;
using System.IO;
using Fingers10.ExcelExport.ActionResults;
using System.Threading.Tasks;
using DomainModels.Helper;
using Microsoft.AspNetCore.Hosting;

namespace UI.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public CategoryController(IUnitOfWork _uow , IHostingEnvironment hostingEnvironment) : base(_uow)
        {
            _hostingEnvironment = hostingEnvironment;

        }


        public IActionResult Index()
        {
            return View(uow.CategoryRepo.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category model)
        {
            try
            {
                uow.CategoryRepo.Add(model);
                uow.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View(uow.CategoryRepo.GetById(id));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Category model)
        {
            try
            {
                model.UpdatedDate = DateTime.Now;
                uow.CategoryRepo.Modify(model);
                uow.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            uow.CategoryRepo.DeleteById(id);
            uow.SaveChanges();
            return RedirectToAction("index");
        }

        public  async Task<IActionResult> Export()
        {
            // Get you IEnumerable<T> data  
            var results = uow.CategoryRepo.GetAll();
            var table = results.ToDataTable();

            return new ExcelResult<Category>(results, "Demo Sheet Name", "Fingers10");
        }


        public FileResult GetFileFormatForUpload()
        {
            DataTable dataTable = new DataTable();
            dataTable.Clear();

            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Reference No", typeof(string));
            dataTable.Columns.Add("Designation", typeof(string));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Mobile", typeof(string));
            dataTable.Columns.Add("Reports To", typeof(string));
            dataTable.Columns.Add("Login Provider", typeof(string));
            dataTable.Columns.Add("Channels", typeof(string));
            dataTable.Columns.Add("Sources", typeof(string));
            dataTable.Columns.Add("Roles", typeof(string));
            dataTable.Columns.Add("Branches", typeof(string));
            dataTable.Columns.Add("Locations", typeof(string));
            dataTable.Columns.Add("Agencies", typeof(string));

            DataRow header = dataTable.NewRow();
            dataTable.Rows.Add(header);

            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");


            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var tempFile = Path.Combine(uploads, "User_Upload_" + Guid.NewGuid().ToString() + ".xls");
             uow.CategoryRepo.SaveExcelData(tempFile, "Sheet1", dataTable);

            return File(tempFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "User_Upload.xls");

        }
    };
}