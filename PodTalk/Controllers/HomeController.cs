using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodTalk.DataContext;
using PodTalk.Models;

namespace PodTalk.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;

    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IActionResult> Index()
    {
        var topics = await _dbContext.Topics.Take(6).ToListAsync();
        

        var model = new HomeViewModel
        {          
            Topics = topics,
         };

        return View(model);
    }


  

    
}
