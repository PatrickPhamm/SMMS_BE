namespace Smmsbe.Services.Models
{
    public class FormResponse
    {
        public int FormId { get; set; }
        public string Title { get; set; }
        public string ClassName { get; set; }
        public string Content { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Type { get; set; }
    }
}
