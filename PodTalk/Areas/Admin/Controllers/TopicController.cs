using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodTalk.DataContext;
using PodTalk.DataContext.Entities;

namespace PodTalk.Areas.Admin.Controllers
{
    public class TopicController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public TopicController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var topic = await _dbContext.Topics
                .Include(t => t.SpeakerTopics)
                .ThenInclude(st => st.Speaker)
                .Include(t => t.Episodes)
                .ToListAsync();

            return View(topic);
        }
        public async Task<IActionResult> Details(int id)
        {
            var topic = await _dbContext.Topics
                .Include(t => t.SpeakerTopics)
                .ThenInclude(st => st.Speaker)
                .Include(t => t.Episodes)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

        public async Task<IActionResult> Delete([FromBody] RequestModel requestModel)
        {
            var topic = await _dbContext.Topics
                .Include(t => t.SpeakerTopics)
                .ThenInclude(st => st.Speaker)
                .Include(t => t.Episodes)
                .FirstOrDefaultAsync(t => t.Id == requestModel.Id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }
        public async Task<IActionResult> Create()
        {
            var speakers = await _dbContext.Speakers.ToListAsync();
            ViewBag.Speakers = speakers;
            return View();
        }
    }
}
