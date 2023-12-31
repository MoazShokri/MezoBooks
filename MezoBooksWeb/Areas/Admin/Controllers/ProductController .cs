﻿using MezoBooks.DataAccess.Repository.IRepository;
using MezoBooks.DataAccess.ViewModels;
using MezoBooks.Models;
using MezoBooks.Uitilty;
using MezoBooksWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace MezoBooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Products.GetAll(includeProperies: "Category").ToList();
            return View(products);
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
            ProductVM prdouctVM = new ()
            {
                CategoryList = _unitOfWork.Categories.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if(id == 0 || id== null)
            {
                //Create
                return View(prdouctVM);

            }
            else
            {
                //Update
                prdouctVM.Product = _unitOfWork.Products.Get(u => u.Id == id, includeProperies: "ProductImages");
            }
            return View(prdouctVM);
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
        {


            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Products.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Products.Update(productVM.Product);
                }

                _unitOfWork.Save();
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (files != null)
                {

                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };

                        if (productVM.Product.ProductImages == null)
                            productVM.Product.ProductImages = new List<ProductImage>();

                        productVM.Product.ProductImages.Add(productImage);

                    }

                    _unitOfWork.Products.Update(productVM.Product);

                    _unitOfWork.Save();

                }


                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Categories.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            }
            return View(productVM);

        }
        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_hostEnvironment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }
        //[HttpPost]
        //public IActionResult Upsert(ProductVM productVM , IFormFile? file)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _hostEnvironment.WebRootPath;
        //        if(file != null)
        //        {
        //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string productPath = Path.Combine(wwwRootPath, @"images\products");
        //            if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
        //            {
        //                // delete the old Image 
        //                var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
        //               if(System.IO.File.Exists(oldImagePath))
        //               {
        //                  System.IO.File.Delete(oldImagePath);
        //               }
        //            }
        //            using(var fileStream = new FileStream(Path.Combine(productPath, fileName) , FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }
        //            productVM.Product.ImageUrl = @"\images\products\" + fileName;
        //        }
        //        if(productVM.Product.Id == 0) 
        //        {
        //            _unitOfWork.Products.Add(productVM.Product);

        //        }else
        //        {
        //            _unitOfWork.Products.Update(productVM.Product);

        //        }
        //        _unitOfWork.Save();
        //        TempData["success"] = "Product Created Successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        productVM.CategoryList = _unitOfWork.Categories.GetAll().Select(u => new SelectListItem
        //            {
        //                Text = u.Name,
        //                Value = u.Id.ToString()
        //            });

        //    }
        //    return View(productVM);

        //}
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
            List<Product> listProductsObj = _unitOfWork.Products.GetAll(includeProperies: "Category").ToList();
            return Json(new { data = listProductsObj });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Products.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}
            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_hostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }

            _unitOfWork.Products.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        #endregion
    }
}
