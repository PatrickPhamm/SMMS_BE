namespace Smmsbe.Services.Models
{
    public class ConsentFormResponse
    {
        public int ConsentFormId { get; set; }
        public int? ParentId { get; set; }
        public string? Status { get; set; }
        public FormResponse Form { get; set; }
    }
}
