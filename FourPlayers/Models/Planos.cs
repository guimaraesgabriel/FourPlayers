using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace FourPlayers.Models
{
    public class Planos
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        public int QuantidadeJogos { get; set; }

        public int Duracao { get; set; }

        public decimal Valor { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Deletado { get; set; }


        //FK



        //ICOLLECTIONS
        public ICollection<Clientes> Clientes { get; set; }
        public ICollection<ClientesHistoricos> HistoricosClientes { get; set; }
    }
}