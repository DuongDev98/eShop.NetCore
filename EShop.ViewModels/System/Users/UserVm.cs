using System.ComponentModel.DataAnnotations;

namespace eShop.ViewModels.System.Users
{
    public class UserVm
    {
        public Guid Id { set; get; }
        [Display(Name = "Tên")]
        public string FirstName { set; get; }
        [Display(Name = "Họ")]
        public string LastName { set; get; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { set; get; }
        [Display(Name = "Điện thoại")]
        public string PhoneNumber { set; get; }
        [Display(Name = "Tài khoản")]
        public string UserName { set; get; }
        [Display(Name = "Email")]
        public string Email { set; get; }
    }
}
