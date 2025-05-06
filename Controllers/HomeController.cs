using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Model;

namespace Laboratorio09___Patrick_Hugo_Ayllón_Rubio.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}