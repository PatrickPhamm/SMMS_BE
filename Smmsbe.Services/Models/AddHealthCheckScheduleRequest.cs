namespace Smmsbe.Services.Models
{
    public class AddHealthCheckScheduleRequest
    {
        public int? FormId { get; set; }
        public int? ManagerId { get; set; }
        public string Name { get; set; }
        public DateTime? CheckDate { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
    }
}
