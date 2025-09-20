using System.ComponentModel.DataAnnotations.Schema;

namespace EmailApp.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public int ReceiverId { get; set; }
        public AppUser Receiver { get; set; }
        public int SenderId { get; set; }
        public AppUser Sender { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; }

        // Yeni özellikler
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDraft { get; set; }
        public string Category { get; set; } 

       
    }
}
