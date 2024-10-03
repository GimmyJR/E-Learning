namespace E_Learning.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string Title { get; set; }
        public int TotalQuestions { get; set; }
    }





}
