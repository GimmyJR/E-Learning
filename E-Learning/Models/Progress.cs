namespace E_Learning.Models
{
    public class Progress
    {
        public int Id { get; set; }

        // Foreign key for User (ApplicationUser)
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public DateTime LastAccessed { get; set; }
    }





}
