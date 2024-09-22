using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB253504Klebeko.UI.Models;

namespace WEB253504Klebeko.UI.Controllers
{
    public class HomeController : Controller
    {
        [ViewData]
        public string Lab { get; set; } = "Лабораторная работа №2";
        public IActionResult Index()
        {
            var data = new List<ListDemo>
            {
                new ListDemo{ Id = 1, Name = "Tom" },
                new ListDemo{ Id = 2, Name = "Alice" },
                new ListDemo{ Id = 3, Name = "Sam" },
                new ListDemo{ Id = 4, Name = "Bob" },
            };

            //var list = new SelectList(data, nameof(ListDemo.Id), nameof(ListDemo.Name));
            //ViewBag.Users = list;
            //return View();

            var list = new SelectList(data, nameof(ListDemo.Id), nameof(ListDemo.Name));

            return View(list);
        }
    }
}
