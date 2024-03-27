using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = this.unitOfWork.Villa.GetAll();
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
                if(villa.Image !=null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                    string imagePath = Path.Combine(webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        villa.Image.CopyTo(fileStream);
                    }

                    villa.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else
                {
                    villa.ImageUrl = "https://placehold.co/600x400";
                }
                this.unitOfWork.Villa.Add(villa);
                this.unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been created successfully.";
                return RedirectToAction("Index", "Villa");
            }

            return View(villa);
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = this.unitOfWork.Villa.Get(x => x.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(webHostEnvironment.WebRootPath, @"images\VillaImage");

                if(!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    var oldImage = Path.Combine(this.webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                    if(System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                }

                villa.ImageUrl = @"\images\VillaImage\" + fileName;
            }
            

            if (ModelState.IsValid && villa.Id > 0)
            {
                this.unitOfWork.Villa.Update(villa);
                this.unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been updated successfully.";
                return RedirectToAction("Index", "Villa");
            }

            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = this.unitOfWork.Villa.Get(x => x.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? objFromDb = this.unitOfWork.Villa.Get(u => u.Id == villa.Id);
             
            if (objFromDb != null)
            {

                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImage = Path.Combine(this.webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }

                this.unitOfWork.Villa.Remove(objFromDb);
                this.unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been deleted successfully.";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "The villa could not be deleted.";
            return View();
        }
    }
}
