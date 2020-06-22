using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class ConsoleController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/console/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.Consoles
                .Where(a => !a.Deletado)
                .Select(a => new
                {
                    a.Id,
                    a.Nome
                })
                .OrderByDescending(a => a.Nome)
                .ToList();

            return lst;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var console = dbContext.Consoles
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Deletado
                })
                .FirstOrDefault();

            if (console == null || console.Deletado == true)
            {
                return NotFound();
            }

            return Ok(console);
        }

        [HttpPut]
        public object Update(Consoles consoleNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var consoleAntigo = dbContext.Consoles.FirstOrDefault(a => a.Id == consoleNovo.Id);

                dbContext.Entry(consoleAntigo).CurrentValues.SetValues(consoleNovo);
                dbContext.SaveChanges();

                tool.MontaLog("Consoles", "console Id: " + consoleNovo.Id + " Nome: " + consoleNovo.Nome + " foi editado com sucesso.", usuarioId, "EDITAR");
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
                consoleId = consoleNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(Consoles console, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Consoles.Add(console);
                    dbContext.SaveChanges();

                    tool.MontaLog("Consoles", "console Id: " + console.Id + " Nome: " + console.Nome + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");

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
                consoleId = console.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var console = dbContext.Consoles.Find(id);

            try
            {
                if (console == null || console.Deletado)
                {
                    return NotFound();
                }

                console.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("Consoles", "console Id: " + console.Id + " Nome: " + console.Nome + " foi deletado com sucesso.", usuarioId, "DELETAR");
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
