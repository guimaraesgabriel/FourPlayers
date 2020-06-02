using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FourPlayers.Models
{
    public class Clientes
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Senha { get; set; }

        [MaxLength(20)]
        public string Telefone { get; set; }

        [MaxLength(20)]
        public string Telefone2 { get; set; }

        public DateTime? DataNascimento { get; set; }

        public DateTime DataCadastro { get; set; }

        [MaxLength(100)]
        public string Hash { get; set; }

        public bool Ativo { get; set; }

        public bool Deletado { get; set; }


        //FK
        public int? PlanoId { get; set; }

        public virtual Planos Planos { get; set; }


        //ICOLLECTION
        public ICollection<ClientesHistoricos> HistoricosClientes { get; set; }
    }
}