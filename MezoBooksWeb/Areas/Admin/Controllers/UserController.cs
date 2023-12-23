using MezoBooks.DataAccess.Repository.IRepository;
using MezoBooks.DataAccess.ViewModels;
using MezoBooks.Models;
using MezoBooks.Uitilty;
using MezoBooksWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MezoBooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUnitOfWork unitOfWork , ApplicationDbContext db,
                              UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            this._unitOfWork = unitOfWork;
            //this._db = db;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}
        //public IActionResult RoleManagment(string userId)
        //{

        //    RoleManagmentVM RoleVM = new RoleManagmentVM()
        //    {
        //        ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperies: "Company"),
        //        RoleList = _roleManager.Roles.Select(i => new SelectListItem
        //        {
        //            Text = i.Name,
        //            Value = i.Name
        //        }),
        //        CompanyList = _unitOfWork.Companies.GetAll().Select(i => new SelectListItem
        //        {
        //            Text = i.Name,
        //            Value = i.Id.ToString()
        //        }),
        //    };

        //    RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userId))
        //            .GetAwaiter().GetResult().FirstOrDefault();
        //    return View(RoleVM);
        //}
        //[HttpPost]
        //public IActionResult RoleManagment(RoleManagmentVM roleManagmentVM)
        //{

        //    string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id))
        //            .GetAwaiter().GetResult().FirstOrDefault();

        //    ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id);


        //    if (!(roleManagmentVM.ApplicationUser.Role == oldRole))
        //    {
        //        //a role was updated
        //        if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company)
        //        {
        //            applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
        //        }
        //        if (oldRole == SD.Role_Company)
        //        {
        //            applicationUser.CompanyId = null;
        //        }
        //        _unitOfWork.ApplicationUser.Update(applicationUser);
        //        _unitOfWork.Save();

        //        _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
        //        _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();

        //    }
        //    else
        //    {
        //        if (oldRole == SD.Role_Company && applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId)
        //        {
        //            applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
        //            _unitOfWork.ApplicationUser.Update(applicationUser);
        //            _unitOfWork.Save();
        //        }
        //    }

        //    return RedirectToAction("Index");
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagment(string userId)
        {

            RoleManagmentVM RoleVM = new RoleManagmentVM()
            {
                ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperies: "Company"),
                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.Companies.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userId))
                    .GetAwaiter().GetResult().FirstOrDefault();
            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagment(RoleManagmentVM roleManagmentVM)
        {

            string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id))
                    .GetAwaiter().GetResult().FirstOrDefault();

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == roleManagmentVM.ApplicationUser.Id);


            if (!(roleManagmentVM.ApplicationUser.Role == oldRole))
            {
                //a role was updated
                if (roleManagmentVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();

                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();

            }
            else
            {
                if (oldRole == SD.Role_Company && applicationUser.CompanyId != roleManagmentVM.ApplicationUser.CompanyId)
                {
                    applicationUser.CompanyId = roleManagmentVM.ApplicationUser.CompanyId;
                    _unitOfWork.ApplicationUser.Update(applicationUser);
                    _unitOfWork.Save();
                }
            }

            return RedirectToAction("Index");
        }

        #region API CALLS
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll(includeProperies: "Company").ToList();
        //    var userRoles = _db.UserRoles.ToList();
        //    var roles = _db.Roles.ToList();
        //    foreach (var user in objUserList)
        //    {
        //        var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
        //        user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
        //        //user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

        //        if (user.Company == null)
        //        {
        //            user.Company = new Company()
        //            {
        //                Name = ""
        //            };
        //        }
        //    }

        //    return Json(new { data = objUserList });
        //}
        //[HttpPost]
        //public IActionResult LockUnlock([FromBody] string id)
        //{

        //    var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
        //    if (objFromDb == null)
        //    {
        //        return Json(new { success = false, message = "Error while Locking/Unlocking" });
        //    }

        //    if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
        //    {
        //        //user is currently locked and we need to unlock them
        //        objFromDb.LockoutEnd = DateTime.Now;
        //    }
        //    else
        //    {
        //        objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
        //    }
        //    _unitOfWork.ApplicationUser.Update(objFromDb);
        //    _unitOfWork.Save();
        //    return Json(new { success = true, message = "Operation Successful" });
        //}


        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll(includeProperies: "Company").ToList();

            foreach (var user in objUserList)
            {

                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }

            return Json(new { data = objUserList });
        }


        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {

            var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked and we need to unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.ApplicationUser.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful" });
        }
        #endregion
    }
}
