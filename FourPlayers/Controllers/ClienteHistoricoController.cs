using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class ClienteHistoricoController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/ClienteHistorico/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.ClientesHistoricos
                .Select(a => new
                {
                    a.Id,
                    a.ClienteId,
                    Cliente = a.Clientes.Nome,
                    a.Data,
                    a.Descricao,
                })
                .ToList();

            return lst;
        }

        [HttpPost]
        public object Save(ClientesHistoricos historico)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.ClientesHistoricos.Add(historico);
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
