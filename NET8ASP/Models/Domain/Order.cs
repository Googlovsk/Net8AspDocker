using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace NET8ASP.Models.Domain
{
    public class Order
    {
        [BindNever] //Не отображается
        public int Id { get; set; }

        [Display(Name = "Имя")]
        [StringLength(50)]
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        [StringLength(50)]
        [Required(ErrorMessage = "Введите фамилию")]
        public string SurName { get; set; }

        [Display(Name = "Адрес")]
        [StringLength(200)]
        [Required(ErrorMessage = "Введите адрес доставки")]
        public string Address { get; set; }

        [Display(Name = "Номер телефона")]
        [StringLength(11)]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Введите номер телефона")]
        public string Phone { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        public string Email { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]//указывается для системных полей, которые не отображаются
        public DateTime OrderTime { get; set; }
        

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
