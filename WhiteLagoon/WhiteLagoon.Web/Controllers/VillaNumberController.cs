using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = this.unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumber = new VillaNumberVM()
            {
                VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            
            return View(villaNumber);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = this.unitOfWork.VillaNumber.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                this.unitOfWork.VillaNumber.Add(obj.VillaNumber);
                this.unitOfWork.Save();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }  

            if(roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists";
            }
            obj.VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumber = new VillaNumberVM()
            {
                VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = this.unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId)
            };

            if (villaNumber.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumber);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                this.unitOfWork.Save();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }

            villaNumberVM.VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumber = new VillaNumberVM()
            {
                VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = this.unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId)
            };

            if (villaNumber.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumber);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = this.unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
             
            if (objFromDb != null)
            {
                this.unitOfWork.VillaNumber.Remove(objFromDb);
                this.unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }

            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
