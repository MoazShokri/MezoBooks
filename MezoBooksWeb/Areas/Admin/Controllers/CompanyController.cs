using MezoBooks.DataAccess.Repository.IRepository;
using MezoBooks.DataAccess.ViewModels;
using MezoBooks.Models;
using MezoBooks.Uitilty;
using MezoBooksWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MezoBooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CompanyController(IUnitOfWork unitOfWork , IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.Companies.GetAll().ToList();
            return View(companies);
        }
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Categories.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.Name,
            //    Value = u.Id.ToString()
            //});
            ////ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
         
            if(id == 0 || id== null)
            {
                //Create
                return View(new Company());

            }
            else
            {
                //Update
                Company company = _unitOfWork.Companies.Get(p=>p.Id == id);
                return View(company);
            }
        
        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
           
            if (ModelState.IsValid)
            {
                
                if(company.Id == 0) 
                {
                    _unitOfWork.Companies.Add(company);

                }else
                {
                    _unitOfWork.Companies.Update(company);

                }
                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction(nameof(Index));
            }
          
            return View(company);

        }
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //        return NotFound();
        //    var productFromDb = _unitOfWork.Products.Get(c => c.Id == id);
        //    if (productFromDb == null)
        //        return NotFound();
        //    return View(productFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Products.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Updated Successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(obj);

        //}
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //        return NotFound();
        //    var productFromDb = _unitOfWork.Products.Get(c => c.Id == id);
        //    if (productFromDb == null)
        //        return NotFound();
        //    return View(productFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{

        //    Product obj = _unitOfWork.Products.Get(c => c.Id == id);
        //    if (obj == null)
        //        return NotFound();

        //    _unitOfWork.Products.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Deleted Successfully";
        //    return RedirectToAction(nameof(Index));



        //}
        #region API CALLS
        public IActionResult GetAll()
        {
            List<Company> ListCompanyObj = _unitOfWork.Companies.GetAll().ToList();
            return Json(new { data = ListCompanyObj });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Companies.Get(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Companies.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
