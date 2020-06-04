using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FourPlayers.Models
{
    public class Reservas
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public DateTime DataExpiracao { get; set; }

        public int ClientId { get; set; }

        [ForeignKey("ClientId")]

        public virtual Clientes Clientes { get; set; }

        public int JogoId { get; set; }

        [ForeignKey("JogoId")]

        public virtual Jogos Jogos { get; set; }
    }
}