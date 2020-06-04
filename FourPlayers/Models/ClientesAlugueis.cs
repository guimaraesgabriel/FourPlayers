using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourPlayers.Models
{
    public class ClientesAlugueis
    {
        [Key]
        public int Id { get; set; }

        public DateTime Datainicio { get; set; }


        //FK
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Clientes Clientes { get; set; }


        public int JogoContaId { get; set; }

        [ForeignKey("JogoContaId")]
        public virtual JogosContas JogosContas { get; set; }


        //ICOLLECTIONS

    }
}