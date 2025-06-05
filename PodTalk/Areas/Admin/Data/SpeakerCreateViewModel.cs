using Microsoft.AspNetCore.Mvc.Rendering;
using PodTalk.DataContext.Entities;

namespace PodTalk.Areas.Admin.Data
{
    public class SpeakerCreateViewModel
    {  
        public required string Name { get; set; }
        public required IFormFile CoverImageFile { get; set; }
        public string? FacebookUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public int EpisodeId { get; set; }  
        public List<Episode> Episodes { get; set; } = new List<Episode>();
        public List<SelectListItem> EpisodeSelectListItems { get; set; } = new List<SelectListItem>();
     
    }
}
