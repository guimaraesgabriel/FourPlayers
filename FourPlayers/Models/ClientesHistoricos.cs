using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourPlayers.Models
{
    public class ClientesHistoricos
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(2000)]
        public string Descricao { get; set; }

        public DateTime Data { get; set; }


        //FK
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Clientes Clientes { get; set; }


        //ICOLLECTIONS

    }
}