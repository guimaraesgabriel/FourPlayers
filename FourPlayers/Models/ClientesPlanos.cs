using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourPlayers.Models
{
    public class ClientesPlanos
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataAdesao { get; set; }

        public DateTime DataExpiracao { get; set; }

        public int TrocasFeitas { get; set; }

        public int TrocasRestantes { get; set; }

        public bool Ativo { get; set; }


        //FK
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Clientes Clientes { get; set; }


        public int PlanoId { get; set; }

        [ForeignKey("PlanoId")]
        public virtual Planos Planos { get; set; }


        //ICOLLECTIONS

    }
}