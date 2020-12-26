namespace Iwentys.Features.Tributes.Models
{
    public class TributeCompleteRequest
    {
        public long TributeId { get; set; }
        public int DifficultLevel { get; set; }
        public int Mark { get; set; }
        public string Comment { get; set; }
    }
}