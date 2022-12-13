using Microsoft.AspNetCore.Identity;

namespace CabServiceManagement.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(
            ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Details");
                return View(model);
            }
            var res = await signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (res.Succeeded)
            {
                if (await userManager.IsInRoleAsync(user, "Driver"))
                {
                    return RedirectToAction("Index", "Home", new { Area = "Driver"});
                }
                else if (await userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Details", "Admin", new { Area = "Admin" });
                }
                return Redirect(nameof(Booking));
            }
            ModelState.AddModelError("", "Invalid Email or  Password");
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = Guid.NewGuid().ToString().Replace("-", ""),
            };
            var role=Convert.ToString(model.Roles);
            var res = await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, role);
            if (res.Succeeded)
            {
                if (await userManager.IsInRoleAsync(user, "Driver"))
                {
                    return RedirectToAction("DriverRegister", "Home", new { Area = "Driver", id = user.Id });
                }
                return RedirectToAction(nameof(Login));
            }
            ModelState.AddModelError("", "An error occured");
            return View(model);

        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

        public async Task<IActionResult> GenerateData()
        {
            await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            await roleManager.CreateAsync(new IdentityRole() { Name = "User" });
            await roleManager.CreateAsync(new IdentityRole() { Name = "Driver" });


            var users = await userManager.GetUsersInRoleAsync("Admin");
            if (users.Count == 0)
            {
                var appUser = new ApplicationUser()
                {
                    FirstName = "Nithu",
                    LastName = "KB",
                    Email = "nithubabu07@gmail.com",
                    UserName = "nithu07",
                };
                var res = await userManager.CreateAsync(appUser, "Kb@123");
                await userManager.AddToRoleAsync(appUser, "Admin");
            }
            return Ok("Data generated");
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await userManager.GetUserAsync(User);
            return View(new EditViewModel()
            {
             FirstName=user.FirstName,
             LastName=user.LastName,
             Email=user.Email
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            
            if (!ModelState.IsValid)
                return View(model);
            var user = await userManager.GetUserAsync(User);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            await userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete()
        {
            var user = await userManager.GetUserAsync(User);
            await signInManager.SignOutAsync();
            var bookingList = await db.Bookings.ToListAsync();
            foreach (var item in bookingList)
            {
                if (item.UserId == user.Id)
                {
                    db.Bookings.Remove(item);
                }
            }
            await userManager.DeleteAsync(user);
            return Redirect("/");
        }
        
        
        [HttpGet]
        public IActionResult Booking()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Booking(BookingViewModel model)
        {
            if (model.From == model.To)
            {
                ModelState.AddModelError(nameof(model.To), "Invalid destination");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.GetUserAsync(User);
            var booking = new Booking()
            {
                To = model.To,
                From = model.From,
                Date = model.Date,
                CarModel=model.CarModel,
                UserId = await userManager.GetUserIdAsync(user)

            };
            await db.AddAsync(booking);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Payment));
        }
        [HttpGet]
       public IActionResult Payment()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Payment(int id)
        {
            return RedirectToAction(nameof(Paid));
        }
        public IActionResult Paid()
        {
            return View();
        }
    }
}