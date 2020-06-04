using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FourPlayers.Models
{
    public class Usuarios
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
        public int GrupoUsuarioId { get; set; }

        public virtual GruposUsuarios GruposUsuarios { get; set; }


        //ICOLLECTIONS
        public ICollection<UsuariosConsoles> Consoles { get; set; }
        public ICollection<MovimentacoesFinanceiras> Despesas { get; set; }
        public ICollection<Log> Log { get; set; }
    }
}