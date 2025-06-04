namespace PodTalk.Areas.Admin.Data
{
    public class TopicCreateViewModel
    {
        public required string Title { get; set; }
        public required IFormFile CoverImageFile { get; set; }

    }
}
