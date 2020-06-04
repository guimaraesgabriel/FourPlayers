using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FourPlayers.Models
{
    public class ClientesAlugueis
    {
        [Key]
        public int Id { get; set; }

        public DateTime Datainicio { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Clientes Clientes { get; set; }

        public int JogoContaId { get; set; }

        [ForeignKey("JogoContaId")]
        public virtual JogosContas JogosContas { get; set; }
    }
}