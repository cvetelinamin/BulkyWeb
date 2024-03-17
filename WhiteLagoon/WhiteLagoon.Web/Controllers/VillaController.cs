using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public VillaController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var villas = this.dbContext.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa)
        {

            if (villa.Name == villa.Description)
            {
                ModelState.AddModelError("", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                this.dbContext.Villas.Add(villa);
                this.dbContext.SaveChanges();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction("Index", "Villa");
            }

            return View(villa);
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
