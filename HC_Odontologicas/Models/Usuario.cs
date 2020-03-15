using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
    public partial class Usuario
    {
        public string Codigo { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string CodigoPerfil { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string Contrasenia { get; set; }
        [Required(ErrorMessage = "Campo requerido.")]
        public string CorreoElectronico { get; set; }
        
        public virtual Perfil Perfil { get; set; }
    }
}
