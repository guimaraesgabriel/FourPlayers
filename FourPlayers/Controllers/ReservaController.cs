using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class ReservaController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/Reserva/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.Reservas
                .Where(a => !a.Deletado)
                .Select(a => new
                {
                    a.Id,
                    a.ClienteId,
                    Cliente = a.Clientes.Nome,
                    a.DataExpiracao,
                    a.DataSolicitacao,
                    a.JogoId,
                    Jogo = a.Jogos.Nome,
                    a.Deletado,
                })
                .OrderByDescending(a => a.Cliente)
                .ToList();

            return lst;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var reserva = dbContext.Reservas
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.ClienteId,
                    Cliente = a.Clientes.Nome,
                    DataExpiracao = a.DataExpiracao.ToString("dd/MM/yyyy"),
                    DataSolicitacao = a.DataSolicitacao.ToString("dd/MM/yyyy"),
                    a.JogoId,
                    Jogo = a.Jogos.Nome,
                    a.Deletado,
                })
                .FirstOrDefault();

            if (reserva == null || reserva.Deletado == true)
            {
                return NotFound();
            }

            return Ok(reserva);
        }

        [HttpPut]
        public object Update(Reservas reservaNova, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var reservaAntiga = dbContext.Reservas.FirstOrDefault(a => a.Id == reservaNova.Id);

                dbContext.Entry(reservaAntiga).CurrentValues.SetValues(reservaNova);
                dbContext.SaveChanges();

                tool.MontaLog("Reservas", "Reserva Id: " + reservaNova.Id + " Cliente: " + reservaNova.Clientes.Nome + " Jogo: " + reservaNova.Jogos.Nome + " foi editada com sucesso.", usuarioId, "EDITAR");
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
                jogoId = reservaNova.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(Reservas reserva, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Reservas.Add(reserva);
                    dbContext.SaveChanges();

                    tool.MontaLog("Reservas", "Reserva Id: " + reserva.Id + " Cliente: " + reserva.Clientes.Nome + " Jogo: " + reserva.Jogos.Nome + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");

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
                jogoId = reserva.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var reserva = dbContext.Reservas.Find(id);

            try
            {
                if (reserva == null)
                {
                    return NotFound();
                }

                reserva.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("Reservas", "Reserva Id: " + reserva.Id + " Cliente: " + reserva.Clientes.Nome + " Jogo: " + reserva.Jogos.Nome + " foi deletado com sucesso.", usuarioId, "DELETAR");
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
