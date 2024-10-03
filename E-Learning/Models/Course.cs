namespace E_Learning.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public CourseLevel Level { get; set; }

        // Foreign key for Instructor (ApplicationUser)
        public string InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; }

        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public TimeSpan Duration { get; set; }
        public int TotalEnrolled { get; set; }
        public string Language { get; set; }
        public string Requirements { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }





}
