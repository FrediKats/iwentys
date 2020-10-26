namespace Iwentys.Models.Transferable
{
    public class AssignmentCreateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? SubjectId { get; set; }
    }
}