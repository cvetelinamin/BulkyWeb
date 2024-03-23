using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository villaRepository;
        public VillaController(IVillaRepository villaRepository)
        {
            this.villaRepository = villaRepository;
        }
        public IActionResult Index()
        {
            var villas = this.villaRepository.GetAll();
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
                this.villaRepository.Add(villa);
                this.villaRepository.Save();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction("Index", "Villa");
            }

            return View(villa);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = this.villaRepository.Get(x => x.Id == villaId);
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
                this.villaRepository.Update(villa);
                this.villaRepository.Save();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction("Index", "Villa");
            }

            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = this.villaRepository.Get(x => x.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? objFromDb = this.villaRepository.Get(u => u.Id == villa.Id);
             
            if (objFromDb != null)
            {
                this.villaRepository.Remove(objFromDb);
                this.villaRepository.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "The villa could not be deleted.";
            return View();
        }
    }
}
