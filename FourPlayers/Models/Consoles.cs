using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourPlayers.Models
{
    public class Consoles
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }


        //FK
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuarios Usuarios { get; set; }


        public int TipoConsoleId { get; set; }

        [ForeignKey("TipoConsoleId")]
        public virtual TiposConsoles TiposConsoles { get; set; }


        //ICOLLECTIONS
        public ICollection<JogosContas> JogosContas { get; set; }
    }
}