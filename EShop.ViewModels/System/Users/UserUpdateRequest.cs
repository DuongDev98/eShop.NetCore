using eShop.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.System.Users
{
    public class UserUpdateRequest
    {
        public Guid Id { get; set; }
        [Display(Name = "Tên")]
        public string FirstName { set; get; }
        [Display(Name = "Họ")]
        public string LastName { set; get; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { set; get; }
        [Display(Name = "Email")]
        public string Email { set; get; }
        [Display(Name = "Điện thoại")]
        public string PhoneNumber { set; get; }
    }
}