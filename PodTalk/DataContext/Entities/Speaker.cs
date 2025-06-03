namespace PodTalk.DataContext.Entities
{
    public class Speaker
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? YoutubeUrl { get; set; }
        public string? TwitterHandle { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public List<SpeakerTopic> SpeakerTopics { get; set; } = [];
        public List<Episode> Episodes { get; set; } = [];

    }
}
