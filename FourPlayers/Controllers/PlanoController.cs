using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class PlanoController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/Plano/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.Planos
                .Where(a => !a.Deletado)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.DataCadastro,
                    a.Duracao,
                    a.QuantidadeJogos,
                    a.Valor,
                    a.Deletado,
                })
                .OrderByDescending(a => a.Nome)
                .ToList();

            return lst;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var plano = dbContext.Planos
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.DataCadastro,
                    a.Duracao,
                    a.QuantidadeJogos,
                    a.Valor,
                    a.Deletado,
                })
                .FirstOrDefault();

            if (plano == null || plano.Deletado == true)
            {
                return NotFound();
            }

            return Ok(plano);
        }

        [HttpPut]
        public object Update(Planos planoNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var planoAntigo = dbContext.Planos.FirstOrDefault(a => a.Id == planoNovo.Id);

                planoNovo.DataCadastro = planoAntigo.DataCadastro;

                dbContext.Entry(planoAntigo).CurrentValues.SetValues(planoNovo);
                dbContext.SaveChanges();

                tool.MontaLog("Planos", "Plano Id: " + planoNovo.Id + " Nome: " + planoNovo.Nome + " foi editado com sucesso.", usuarioId, "EDITAR");
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
                planoId = planoNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(Planos plano, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Planos.Add(plano);
                    dbContext.SaveChanges();

                    tool.MontaLog("Planos", "Plano Id: " + plano.Id + " Nome: " + plano.Nome + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");

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
                planoId = plano.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var plano = dbContext.Planos.Find(id);

            try
            {
                if (plano == null || plano.Deletado)
                {
                    return NotFound();
                }

                plano.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("Planos", "Plano Id: " + plano.Id + " Nome: " + plano.Nome + " foi deletado com sucesso.", usuarioId, "DELETAR");
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
