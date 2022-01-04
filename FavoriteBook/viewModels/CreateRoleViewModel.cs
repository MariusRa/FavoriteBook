using System.ComponentModel.DataAnnotations;


namespace FavoriteBook.viewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
