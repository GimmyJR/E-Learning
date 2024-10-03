namespace E_Learning.Models
{
    public class Payment
    {
        public int Id { get; set; }

        // Foreign key for User (ApplicationUser)
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }





}
