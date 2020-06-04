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
        public string GUID { get; set; }

        public bool Ativo { get; set; }

        public bool Deletado { get; set; }


        //FK


        //ICOLLECTIONS
        public ICollection<ClientesAlugueis> ClientesAlugueis { get; set; }
        public ICollection<ClientesHistoricos> ClientesHistoricos { get; set; }
        public ICollection<ClientesPlanos> ClientesPlanos { get; set; }
        public ICollection<Reservas> Reservas { get; set; }
    }
}