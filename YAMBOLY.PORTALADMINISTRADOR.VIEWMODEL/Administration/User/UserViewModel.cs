using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YAMBOLY.PORTALADMINISTRADOR.HELPER;

namespace YAMBOLY.PORTALADMINISTRADOR.VIEWMODEL.Administration.User
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            Roles = new List<JsonEntity>();
        }
        public int? UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Pass { get; set; }

        [Required]
        [Display(Name ="Rol")]
        public int? RolId { get; set; }
        public List<JsonEntity> Roles { get; set; }


        [Required]
        public string Nombres { get; set; }

        [Required]
        
        public string Documento { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Correo { get; set; }

        public string Telefono { get; set; }
    }
}
