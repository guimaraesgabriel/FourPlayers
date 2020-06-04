using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FourPlayers.Models
{
    public class Despesas
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public String Descricao { get; set; }

        public Decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public int? UsuarioId { get; set; }

        public virtual Usuarios Usuarios { get; set; }
    }
}