namespace ScholarshipManagementAPI.DTOs.Ngo
{
    public class ApprovalRequestDto
    {
        // entity id can be course-type, course, course-req, school, etc.
        // depending on the approval context

        public long EntityId { get; set; }
        public int ApprovalStatus { get; set; }
    }
}
