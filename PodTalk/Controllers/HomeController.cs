using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PodTalk.Models;

namespace PodTalk.Controllers;

public class HomeController : Controller
{
   
    public IActionResult Index()
    {
        return View();
    }

    
}
