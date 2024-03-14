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

            if(villa.Name == villa.Description)
            {
                ModelState.AddModelError("", "The description cannot exactly match the Name.");
            }
            if(ModelState.IsValid)
            {
                this.dbContext.Villas.Add(villa);
                this.dbContext.SaveChanges();

                return RedirectToAction("Index", "Villa");
            }

            return View(villa);
        }
    }
}
