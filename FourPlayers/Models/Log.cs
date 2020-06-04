using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FourPlayers.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public DateTime Data { get; set; }

        [MaxLength(2000)]
        public string Descricao { get; set; }

        [MaxLength(100)]
        public string Tela { get; set; }

        [MaxLength(100)]
        public string Acao { get; set; }


        //FK
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuarios Usuario { get; set; }


        //ICOLLECTIONS

    }
}