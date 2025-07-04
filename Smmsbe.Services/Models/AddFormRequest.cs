using Smmsbe.Services.Enum;

namespace Smmsbe.Services.Models
{
    public class AddFormRequest
    {
        public string ClassName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public FormType Type { get; set; }

        public List<int> ParentIds { get; set; } = new List<int>(); // Danh sách ParentId để tạo ConsentForm
    }
}
