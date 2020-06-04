using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FourPlayers.Models
{
    public class Consoles
    {
        [Key]
        public int Id { get; set; }

        [MaxLength (100)]

        public String Nome { get; set;  }

        public int UsuarioId { get; set; }

        [ForeignKey("UsarioId")]
        public virtual Usuarios Usuarios { get; set; }

        public int TipoId { get; set; }

        [ForeignKey("TipoId")]
        public virtual TiposConsoles TiposConsoles { get; set; }
    }
}