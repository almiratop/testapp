using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using test.Models;
namespace test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var csvData = reader.ReadToEnd();
                    var lines = csvData.Split('\n');

                    foreach (var line in lines.Skip(1)) // Пропустить заголовок
                    {
                        var columns = line.Split(',');

                        var user = new User
                        {
                            FirstName = columns[0],
                            LastName = columns[1],
                            Email = columns[2]
                        };

                        _context.Users.Add(user);
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index"); // Перенаправление после загрузки
            }

            return View();
        }
    }
}
