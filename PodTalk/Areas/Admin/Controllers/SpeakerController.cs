using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PodTalk.Areas.Admin.Data;
using PodTalk.Areas.Admin.Extensions;
using PodTalk.DataContext;
using PodTalk.DataContext.Entities;

namespace PodTalk.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpeakerController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public SpeakerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var speakers = await _dbContext.Speakers
                .Include(s => s.Episodes)
                .ToListAsync();


            return View(speakers);
        }
        public async Task<IActionResult> Details(int id)
        {
            var speaker = await _dbContext.Speakers
                .Include(s => s.Episodes)
                .SingleOrDefaultAsync(s => s.Id == id);
            if (speaker == null)
            {
                return NotFound();
            }
            return View(speaker);
        }
        public async Task<IActionResult> Delete([FromBody] RequestModel requestModel)
        {
            var speaker = await _dbContext.Speakers
                .FirstOrDefaultAsync(x => x.Id == requestModel.Id);
            if (speaker == null) return NotFound();
            var removedSpeaker = _dbContext.Speakers.Remove(speaker);
            await _dbContext.SaveChangesAsync();
            if (removedSpeaker != null)
            {
                System.IO.File.Delete(Path.Combine(FilePathConstants.SpeakerPath, speaker.ImageUrl));
            }
            return Json(removedSpeaker.Entity);
        }
        public async Task<IActionResult> Create()
        {
            var episodes = await _dbContext.Episodes.ToListAsync();
            var episodeSelectListItems = episodes.Select(e => new SelectListItem(e.Title, e.Id.ToString())).ToList();

            var speakerCreateViewModel = new SpeakerCreateViewModel()
            {
                Name = "",
                EpisodeSelectListItems = episodeSelectListItems,
                CoverImageFile = null,

            };
            return View(speakerCreateViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateViewModel model)
        {
            var episodes = await _dbContext.Episodes.ToListAsync();
            var episodeSelectListItems = episodes.Select(e => new SelectListItem(e.Title, e.Id.ToString())).ToList();

            if (!ModelState.IsValid)
            {
                model.EpisodeSelectListItems = episodeSelectListItems;
                return View(model);
            }

            if (!model.CoverImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "Sekil secilmelidir!");
                model.EpisodeSelectListItems = episodeSelectListItems;

                return View(model);
            }

            if (!model.CoverImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageFile", "Sekil hecmi 1mb-dan cox ola bilmez");
                model.EpisodeSelectListItems = episodeSelectListItems;


                return View(model);
            }

            var unicalCoverImageFileName = await model.CoverImageFile.GenerateFile(FilePathConstants.SpeakerPath);

            Speaker speaker = new Speaker
            {
                Name = model.Name,
                ImageUrl = unicalCoverImageFileName,
                FacebookUrl = model.FacebookUrl,
                YoutubeUrl = model.YoutubeUrl,
                TwitterHandle = model.TwitterUrl,
                LinkedInUrl = model.LinkedInUrl,
                InstagramUrl = model.InstagramUrl,
                WebsiteUrl = model.WebsiteUrl,

            };

            if (model.CoverImageFile != null && model.CoverImageFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{model.CoverImageFile.FileName}";
                var filePath = Path.Combine(FilePathConstants.SpeakerPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.CoverImageFile.CopyToAsync(stream);
                }
                speaker.ImageUrl = fileName;
            }
            _dbContext.Speakers.Add(speaker);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
