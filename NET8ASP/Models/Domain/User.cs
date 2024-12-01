using System.ComponentModel.DataAnnotations;
using NET8ASP.Models.Domain.Cart;

namespace NET8ASP.Models.Domain
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите ФИО")]
        [StringLength(100, ErrorMessage = "ФИО не может превышать 100 символов")]
        public string Fio { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [Phone(ErrorMessage = "Введите корректный номер телефона")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(50, ErrorMessage = "Логин не может превышать 50 символов")]
        public string Login {  get; set; }
        [Required(ErrorMessage = "Это поле обязательно")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Пароль должен быть от 8 до 100 символов")]
        public string Password { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual NET8ASP.Models.Domain.Cart.Cart Cart { get; set; }

    }
}
