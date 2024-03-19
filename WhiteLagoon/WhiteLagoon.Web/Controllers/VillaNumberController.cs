using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public VillaNumberController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var villaNumbers = this.dbContext.VillaNumbers.Include(u => u.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumber = new VillaNumberVM()
            {
                VillaList = this.dbContext.Villas.ToList().Select(u => new SelectListItem
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
            bool roomNumberExists = this.dbContext.VillaNumbers.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                this.dbContext.VillaNumbers.Add(obj.VillaNumber);
                this.dbContext.SaveChanges();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }  

            if(roomNumberExists)
            {
                TempData["error"] = "The villa Number already exists";
            }
            obj.VillaList = this.dbContext.Villas.ToList().Select(u => new SelectListItem
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
                VillaList = this.dbContext.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = this.dbContext.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
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
                this.dbContext.VillaNumbers.Update(villaNumberVM.VillaNumber);
                this.dbContext.SaveChanges();
                TempData["success"] = "The villa number has been updated successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }

            villaNumberVM.VillaList = this.dbContext.Villas.ToList().Select(u => new SelectListItem
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
                VillaList = this.dbContext.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = this.dbContext.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
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
            VillaNumber? objFromDb = this.dbContext.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
             
            if (objFromDb != null)
            {
                this.dbContext.VillaNumbers.Remove(objFromDb);
                this.dbContext.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }

            TempData["error"] = "The villa number could not be deleted.";
            return View();
        }
    }
}
