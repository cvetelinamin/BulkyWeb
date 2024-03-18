using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

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
            var villaNumbers = this.dbContext.VillaNumbers.ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = this.dbContext.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            ViewData["VillaList"] = list;
            return View();
        }

        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
            if (ModelState.IsValid)
            {
                this.dbContext.VillaNumbers.Add(obj);
                this.dbContext.SaveChanges();
                TempData["success"] = "The villa number has been created successfully.";
                return RedirectToAction("Index", "VillaNumber");
            }

            return View(obj);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = this.dbContext.Villas.FirstOrDefault(x => x.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {

            if (ModelState.IsValid && villa.Id > 0)
            {
                this.dbContext.Villas.Update(villa);
                this.dbContext.SaveChanges();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction("Index", "Villa");
            }

            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = this.dbContext.Villas.FirstOrDefault(x => x.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? objFromDb = this.dbContext.Villas.FirstOrDefault(u => u.Id == villa.Id);
             
            if (objFromDb != null)
            {
                this.dbContext.Villas.Remove(objFromDb);
                this.dbContext.SaveChanges();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "The villa could not be deleted.";
            return View();
        }
    }
}
