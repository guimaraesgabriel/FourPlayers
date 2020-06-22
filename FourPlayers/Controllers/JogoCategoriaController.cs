using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class JogoCategoriaController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/JogoCategoria/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.JogosCategorias
                .Select(a => new
                {
                    a.Id,
                    a.JogoId,
                    Jogo = a.Jogos.Nome,
                    a.CategoriaId,
                    Categoria = a.Categorias.Nome,
                })
                .ToList();

            return lst;
        }

        [HttpGet]
        [Route("api/JogoCategoria/GetAllJogo")]
        public IEnumerable<object> GetAllJogo(int jogoId)
        {
            var lst = dbContext.JogosCategorias
                .Where(a => a.JogoId == jogoId)
                .Select(a => new
                {
                    a.Id,
                    a.JogoId,
                    Jogo = a.Jogos.Nome,
                    a.CategoriaId,
                    Categoria = a.Categorias.Nome,
                })
                .ToList();

            return lst;
        }

        [HttpPost]
        public object Save(List<JogosCategorias> lstJogosCategorias, int usuarioId)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (lstJogosCategorias.Count == 0)
                    {
                        return false;
                    }

                    var jogoId = lstJogosCategorias.FirstOrDefault().JogoId;

                    //REMOVER AS CATEGORIAS ANTERIORES DO JOGO
                    var lstRemover = dbContext.JogosCategorias
                        .Where(a => a.JogoId == jogoId)
                        .ToList();

                    dbContext.JogosCategorias.RemoveRange(lstRemover);
                    dbContext.SaveChanges();

                    //ADICIONAR NOVOS
                    foreach (var jogoCategoria in lstJogosCategorias)
                    {
                        dbContext.JogosCategorias.Add(jogoCategoria);
                        dbContext.SaveChanges();

                        tool.MontaLog("JogosCategorias", "Jogo Categoria Id: " + jogoCategoria.Id + " Jogo: " + jogoCategoria.Jogos.Nome + " Categoria: " + jogoCategoria.Categorias.Nome + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");
                    }

                    transaction.Commit();

                    var obj = new
                    {
                        statusReq = true,
                        erro = ""
                    };

                    return obj;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    var obj = new
                    {
                        statusReq = false,
                        erro = ex.Message
                    };

                    return obj;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
