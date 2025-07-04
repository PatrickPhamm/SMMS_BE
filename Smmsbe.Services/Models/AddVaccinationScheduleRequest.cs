namespace Smmsbe.Services.Models
{
    public class AddVaccinationScheduleRequest
    {

        public int? FormId { get; set; }
        public int? ManagerId { get; set; }
        public string Name { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
    }
}
