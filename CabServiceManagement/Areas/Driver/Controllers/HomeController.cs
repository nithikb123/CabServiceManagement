using CabServiceManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CabServiceManagement.Areas.Driver.Controllers
{
    [Area("Driver")]
    //[Authorize(Roles = "Driver")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        public async Task<ActionResult> Index()
        {
            return View(await db.Bookings.ToListAsync());
        }
        [HttpGet]
        public IActionResult DriverRegister()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DriverRegister([FromForm]DriverViewModel data, [FromRoute]string id)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var user = new DriverDetails()
            {
                CabName = data.CabName,
                CabNumber = data.CabNumber,
                LicenceNumber = data.LicenceNumber,
                Model = data.Model,
                DriverId = id
            };
            await db.AddAsync(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Login", "Home", new { Area = "Accounts" });
        }
        
        public async Task<IActionResult> Accept(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var driver = await db.DriverDetail.FirstOrDefaultAsync(x => x.DriverId == user.Id);
            var booking = await db.Bookings.FindAsync(id);
            booking.DriverId = driver.Id;
            return View();
        }
    }
}
