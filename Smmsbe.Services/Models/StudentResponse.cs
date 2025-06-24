namespace Smmsbe.Services.Models
{
    public class StudentResponse
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string ClassName { get; set; }
        public string Gender { get; set; }
        public string StudentNumber { get; set; }
        public ParentResponse Parent { get; set; }
    }
}
