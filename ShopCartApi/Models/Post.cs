namespace ShopCartApi.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string UserHobby { get; set; } = "";
        public DateTime DateCreatedUtc { get; set; }
        public DateTime? DateEditedUtc { get; set; }
        public User User { get; set; }

        public Post()
        {

        }
    }
}
