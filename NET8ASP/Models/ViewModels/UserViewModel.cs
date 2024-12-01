using NET8ASP.Models.Domain;

namespace NET8ASP.Models.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
