using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FourPlayers.Models
{
    public class Planos
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        public int QuantidadeTrocas { get; set; }

        public int Duracao { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Deletado { get; set; }


        //FK



        //ICOLLECTIONS
        public ICollection<ClientesPlanos> ClientesPlanos { get; set; }
    }
}