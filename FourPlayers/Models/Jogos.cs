using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FourPlayers.Models
{
    public class Jogos
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Nome { get; set; }

        [MaxLength(255)]
        public string CapaURL { get; set; }

        [MaxLength(2000)]
        public string Sinopse { get; set; }

        public int Pontuacao { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }

        public bool Deletado { get; set; }


        //FK


        //ICOLLECTIONS
        public ICollection<JogosImagens> ImagensJogos { get; set; }
        public ICollection<JogosCategorias> JogosCategorias { get; set; }
        public ICollection<JogosContas> JogosContas { get; set; }
        public ICollection<Reservas> Reservas { get; set; }
    }
}