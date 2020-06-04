using System;
using System.ComponentModel.DataAnnotations;

namespace FourPlayers.Models
{
    public class MovimentacoesFinanceiras
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Descricao { get; set; }

        [MaxLength(50)]
        public string Tipo { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }


        //FK
        public int? UsuarioId { get; set; }

        public virtual Usuarios Usuarios { get; set; }


        //ICOLLECTIONS

    }
}