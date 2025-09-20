namespace EmailApp.Models
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }   // Güncellenecek kullanıcıyı bulmak için
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
