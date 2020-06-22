using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class UsuarioConsoleController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/Jogo/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.UsuariosConsoles
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.ConsoleId,
                    Console = a.Consoles.Nome,
                    a.UsuarioId,
                    Usuario = a.Usuarios.Nome,
                })
                .OrderByDescending(a => a.Nome)
                .ToList();

            return lst;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var jogo = dbContext.UsuariosConsoles
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.ConsoleId,
                    Console = a.Consoles.Nome,
                    a.UsuarioId,
                    Usuario = a.Usuarios.Nome,
                })
                .FirstOrDefault();

            if (jogo == null)
            {
                return NotFound();
            }

            return Ok(jogo);
        }

        [HttpPut]
        public object Update(UsuariosConsoles usuarioConsoleNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var usuarioConsoleAntigo = dbContext.UsuariosConsoles.FirstOrDefault(a => a.Id == usuarioConsoleNovo.Id);

                dbContext.Entry(usuarioConsoleAntigo).CurrentValues.SetValues(usuarioConsoleNovo);
                dbContext.SaveChanges();

                tool.MontaLog(
                    "UsuariosConsoles",
                    "Usuario Console Id: " + usuarioConsoleNovo.Id +
                    " Nome: " + usuarioConsoleNovo.Nome + 
                    " Console: " + usuarioConsoleNovo.Consoles.Nome + 
                    " Usuário: " + usuarioConsoleNovo.Usuarios.Nome + 
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
                usuarioConsoleId = usuarioConsoleNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(UsuariosConsoles usuarioConsole, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.UsuariosConsoles.Add(usuarioConsole);
                    dbContext.SaveChanges();

                    tool.MontaLog(
                         "UsuariosConsoles",
                         "Usuario Console Id: " + usuarioConsole.Id +
                         " Nome: " + usuarioConsole.Nome +
                         " Console: " + usuarioConsole.Consoles.Nome +
                         " Usuário: " + usuarioConsole.Usuarios.Nome +
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
                usuarioConsoleId = usuarioConsole.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var usuarioConsole = dbContext.UsuariosConsoles.Find(id);

            try
            {
                if (usuarioConsole == null)
                {
                    return NotFound();
                }

                var console = usuarioConsole.Consoles.Nome;
                var usuario = usuarioConsole.Usuarios.Nome;

                dbContext.UsuariosConsoles.Remove(usuarioConsole);
                dbContext.SaveChanges();

                tool.MontaLog(
                     "UsuariosConsoles",
                     "Usuario Console Id: " + usuarioConsole.Id +
                     " Nome: " + usuarioConsole.Nome +
                     " Console: " + console +
                     " Usuário: " + usuario +
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
