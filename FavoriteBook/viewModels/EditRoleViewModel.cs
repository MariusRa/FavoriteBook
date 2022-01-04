using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FavoriteBook.viewModels
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage ="Role Name is required")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }

        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
    }
}
