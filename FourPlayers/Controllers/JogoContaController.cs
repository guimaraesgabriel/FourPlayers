using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class JogoContaController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/Jogo/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.JogosContas
                .Select(a => new
                {
                    a.Id,
                    a.ConsoleId,
                    Console = a.Consoles.Nome,
                    a.JogoId,
                    Jogo = a.Jogos.Nome,
                    a.StatusId,
                    a.StatusJogos.Nome,
                    a.TipoContaId,
                    Tipo = a.TiposContas.Nome,
                })
                .OrderByDescending(a => a.Console)
                .ToList();

            return lst;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var jogoContas = dbContext.JogosContas
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.ConsoleId,
                    Console = a.Consoles.Nome,
                    a.JogoId,
                    Jogo = a.Jogos.Nome,
                    a.StatusId,
                    a.StatusJogos.Nome,
                    a.TipoContaId,
                    Tipo = a.TiposContas.Nome,
                })
                .FirstOrDefault();

            if (jogoContas == null)
            {
                return NotFound();
            }

            return Ok(jogoContas);
        }

        [HttpPut]
        public object Update(JogosContas jogoContasNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var jogoContasAntigo = dbContext.JogosContas.FirstOrDefault(a => a.Id == jogoContasNovo.Id);

                dbContext.Entry(jogoContasAntigo).CurrentValues.SetValues(jogoContasNovo);
                dbContext.SaveChanges();

                tool.MontaLog(
                    "JogosContas",
                    "Jogo Conta Id: " + jogoContasNovo.Id + 
                    " Jogo: " + jogoContasNovo.Jogos.Nome + 
                    " Console: " + jogoContasNovo.Consoles.Nome + 
                    " foi editado com sucesso.",
                    usuarioId,
                    "EDITAR"
                );
            }
            catch (Exception ex)
            {
                atualiza = false;
                erro = ex.Message;
            }

            var objRetorno = new
            {
                statusReq = atualiza,
                erro,
                jogoId = jogoContasNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(JogosContas jogoContas, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.JogosContas.Add(jogoContas);
                    dbContext.SaveChanges();

                    tool.MontaLog(
                       "JogosContas",
                       "Jogo Conta Id: " + jogoContas.Id +
                       " Jogo: " + jogoContas.Jogos.Nome +
                       " Console: " + jogoContas.Consoles.Nome +
                       " foi adicionado com sucesso.",
                       usuarioId,
                       "ADICIONAR"
                   );

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    insere = false;
                    erro = ex.Message;
                }
            }

            var objRetorno = new
            {
                statusReq = insere,
                erro,
                jogoContaId = jogoContas.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var jogoContas = dbContext.JogosContas.Find(id);

            try
            {
                if (jogoContas == null)
                {
                    return NotFound();
                }

                var jogo = jogoContas.Jogos.Nome;
                var console = jogoContas.Consoles.Nome;

                dbContext.JogosContas.Remove(jogoContas);
                dbContext.SaveChanges();

                tool.MontaLog(
                    "JogosContas",
                    "Jogo Conta Id: " + jogoContas.Id +
                    " Jogo: " + jogo +
                    " Console: " + console +
                    " foi deletado com sucesso.",
                    usuarioId,
                    "DELETAR"
                );
            }
            catch (Exception ex)
            {
                delete = false;
                erro = ex.Message;
            }

            var objRetorno = new
            {
                statusReq = delete,
                erro
            };

            return objRetorno;
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
