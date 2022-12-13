using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CabServiceManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AdminController/Details/5
        public  async Task<ActionResult> Details(int id)
        {
            return View(await db.User.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await db.User.FindAsync(id);
            //var roles = await userManager.GetRolesAsync(user);
            return View(new EditViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model,string id)
        {

            if (!ModelState.IsValid)
                return View(model);
            var user = await db.User.FindAsync(id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            db.User.Update(user);
            db.SaveChanges();
            return RedirectToAction(nameof(Details));
        }
        public async Task<IActionResult> Delete(string id)
        {
            var user = await db.User.FindAsync(id);
            var bookingList = await db.Bookings.ToListAsync();
            foreach (var item in bookingList)
            {
                if (item.UserId == user.Id)
                {
                    db.Bookings.Remove(item);
                }
            }
            var driver = await db.DriverDetail.FirstOrDefaultAsync(m => m.DriverId == id);
            await userManager.DeleteAsync(user);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Details));
        }

    }
}
