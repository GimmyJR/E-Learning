namespace E_Learning.Models
{
    public class Message
    {
        public int Id { get; set; }

        // Foreign key for Sender (ApplicationUser)
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        // Foreign key for Receiver (ApplicationUser)
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

        public string MessageText { get; set; }
        public DateTime SentDate { get; set; }
    }





}
