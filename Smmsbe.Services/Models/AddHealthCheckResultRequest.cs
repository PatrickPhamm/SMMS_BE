namespace Smmsbe.Services.Models
{
    public class AddHealthCheckResultRequest
    {
        public int? HealthCheckScheduleId { get; set; }
        public int? NurseId { get; set; }
        public int? HealthProfileId { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string LeftVision { get; set; }
        public string RightVision { get; set; }
        public string Result { get; set; }
        public string Note { get; set; }
    }
}
