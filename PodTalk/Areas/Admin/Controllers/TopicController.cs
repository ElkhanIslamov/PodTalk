using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PodTalk.Areas.Admin.Data;
using PodTalk.Areas.Admin.Extensions;
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
                .Include(t => t.Episodes)
                .ToListAsync();

            return View(topic);
        }
        public async Task<IActionResult> Details(int id)
        {
            var topic = await _dbContext.Topics
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
            var topicCreateViewModel = new TopicCreateViewModel()
            {
                Title = "",
                CoverImageFile = null
            };
            return View(topicCreateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TopicCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!model.CoverImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Sekil secilmelidir!");
                return View(model);
            }

            if (!model.CoverImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");
                return View(model);
            }

            var unicalCoverImageFileName = await model.CoverImageFile.GenerateFile(FilePathConstants.TopicPath);

            var topic = new Topic
            {
                Title = model.Title,
                ImageUrl = unicalCoverImageFileName
            };
            await _dbContext.Topics.AddAsync(topic);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
