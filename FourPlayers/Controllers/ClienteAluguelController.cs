using FourPlayers.Context;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class ClienteAluguelController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();

        [HttpGet]
        [Route("api/ClienteAluguel/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.ClientesAlugueis
                .Select(a => new
                {
                    a.Id,
                    a.ClienteId,
                    Cliente = a.Clientes.Nome,
                    a.DataAluguel,
                    a.DataEntrega,
                    a.JogoContaId,
                    Jogo = a.JogosContas.Jogos.Nome,
                })
                .ToList();

            return lst;
        }

        [HttpPost]
        public object Save(ClientesAlugueis aluguel)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    aluguel.DataAluguel = DateTime.Now;

                    dbContext.ClientesAlugueis.Add(aluguel);
                    dbContext.SaveChanges();

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
