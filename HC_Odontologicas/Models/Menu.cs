using System;
using System.Collections.Generic;

namespace HC_Odontologicas.Models
{
    public partial class Menu
    {
        public Menu()
        {
            SubMenu = new List<Menu>();
            PerfilDetalle = new HashSet<PerfilDetalle>();
        }

        public string Codigo { get; set; }
        public string CodigoMenu { get; set; }
        public string Nombre { get; set; }
        public string Accion { get; set; }
        public string Icono { get; set; }
        public byte? Orden { get; set; }
        public bool? Visible { get; set; }
        public bool Ver { get; set; }
        public bool Crear { get; set; }
        public bool Editar { get; set; }
        public bool Eliminar { get; set; }
        public bool Exportar { get; set; }
        public bool Importar { get; set; }

        public virtual Menu MenuCodigo { get; set; }
        public virtual List<Menu> SubMenu { get; set; }
        public virtual ICollection<PerfilDetalle> PerfilDetalle { get; set; }
    }
}
