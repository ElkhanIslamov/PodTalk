namespace PodTalk.Areas.Admin.Data
{
    public class TopicUpdateViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? CoverImageUrl { get; set; }
        public IFormFile? CoverImageFile { get; set; }
    }
}
