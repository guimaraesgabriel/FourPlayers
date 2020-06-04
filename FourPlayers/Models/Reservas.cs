using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourPlayers.Models
{
    public class Reservas
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public DateTime DataExpiracao { get; set; }


        //FK
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Clientes Clientes { get; set; }


        public int JogoId { get; set; }

        [ForeignKey("JogoId")]
        public virtual Jogos Jogos { get; set; }


        //ICOLLECTIONS

    }
}