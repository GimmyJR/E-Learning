namespace E_Learning.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public string QuestionText { get; set; }
        public int Order { get; set; }
    }





}
