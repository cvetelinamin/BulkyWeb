using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenities = this.unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }

        public IActionResult Create()
        {
            AmenityVM amenity = new AmenityVM()
            {
                VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            
            return View(amenity);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
          
            if (ModelState.IsValid)
            {
                this.unitOfWork.Amenity.Add(obj.Amenity);
                this.unitOfWork.Save();
                TempData["success"] = "The amenity has been created successfully.";
                return RedirectToAction("Index", "Amenity");
            }  
     
            obj.VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(obj);
        }

        public IActionResult Update(int amenityId)
        {
            AmenityVM amenity = new AmenityVM()
            {
                VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = this.unitOfWork.Amenity.Get(x => x.Id == amenityId)
            };

            if (amenity.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenity);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                this.unitOfWork.Amenity.Update(amenityVM.Amenity);
                this.unitOfWork.Save();
                TempData["success"] = "The amenity has been updated successfully.";
                return RedirectToAction("Index", "Amenity");
            }

            amenityVM.VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(amenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenity = new AmenityVM()
            {
                VillaList = this.unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = this.unitOfWork.Amenity.Get(x => x.Id == amenityId)
            };

            if (amenity.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenity);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = this.unitOfWork.Amenity.Get(u => u.Id == amenityVM.Amenity.Id);
             
            if (objFromDb != null)
            {
                this.unitOfWork.Amenity.Remove(objFromDb);
                this.unitOfWork.Save();
                TempData["success"] = "The amenity has been deleted successfully.";
                return RedirectToAction("Index", "Amenity");
            }

            TempData["error"] = "The amenity could not be deleted.";
            return View();
        }
    }
}
