using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            .SingleOrDefaultAsync(t => t.Id == id);


            return View(topic);
        }
        public async Task<IActionResult> Delete([FromBody] RequestModel requestModel)
        {
            var topic = await _dbContext.Topics
           .FirstOrDefaultAsync(x => x.Id == requestModel.Id);

            if (topic == null) return NotFound();

            var removedTopic = _dbContext.Topics.Remove(topic);
            await _dbContext.SaveChangesAsync();

            if (removedTopic != null)
            {
                System.IO.File.Delete(Path.Combine(FilePathConstants.TopicPath, topic.ImageUrl));

                foreach (var item in topic.ImageUrl)
                {
                    System.IO.File.Delete(Path.Combine(FilePathConstants.TopicPath));
                }
            }

            return Json(removedTopic.Entity);
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
        public async Task<IActionResult> Update(int id)
        {
            var topic = await _dbContext.Topics
               .FirstOrDefaultAsync(x => x.Id == id);                      

            if (topic == null) return NotFound();

            TopicUpdateViewModel updateViewModel = new TopicUpdateViewModel
            {
                Title = topic.Title,
                CoverImageUrl = topic.ImageUrl,
             
            };

            return View(updateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TopicUpdateViewModel model)
        {
            var topic = await _dbContext.Topics.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (topic == null) return NotFound();

            if (!ModelState.IsValid)
            {
                 return View(model);
            }

            topic.Title = model.Title;
            topic.ImageUrl = model.CoverImageUrl;
         

            if (model.CoverImageFile != null)
            {
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

                if (topic.ImageUrl != null)
                {
                    System.IO.File.Delete(Path.Combine(FilePathConstants.TopicPath, topic.ImageUrl));
                }

                topic.ImageUrl = unicalCoverImageFileName;
            }
    

            _dbContext.Topics.Update(topic);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

      

    }
}
