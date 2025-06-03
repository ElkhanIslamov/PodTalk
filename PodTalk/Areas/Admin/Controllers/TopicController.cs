using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodTalk.DataContext;

namespace PodTalk.Areas.Admin.Controllers
{
    public class TopicController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public TopicController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task <IActionResult> Index()
        {
            var topic = await _dbContext.Topics
                .Include(t => t.SpeakerTopics)
                .ThenInclude(st => st.Speaker)
                .Include(t => t.Episodes)
                .ToListAsync();

            return View(topic);
        }
    }
}
