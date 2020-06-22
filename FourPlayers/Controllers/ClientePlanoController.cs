using FourPlayers.Context;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class ClientePlanoController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();

        [HttpGet]
        [Route("api/ClientePlano/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.ClientesPlanos
                .Select(a => new
                {
                    a.Id,
                    a.ClienteId,
                    Cliente = a.Clientes.Nome,
                    a.Ativo,
                    a.DataAdesao,
                    a.DataExpiracao,
                    a.PlanoId,
                    Plano = a.Planos.Nome,
                    a.TrocasFeitas,
                    a.TrocasRestantes,
                })
                .ToList();

            return lst;
        }

        [HttpPost]
        public object Save(ClientesPlanos plano)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.ClientesPlanos.Add(plano);
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
