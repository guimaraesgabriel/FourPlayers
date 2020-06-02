using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class GrupoUsuarioController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/GrupoUsuario/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.GruposUsuarios
                .Where(a => !a.Deletado)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Ativo,
                    a.Deletado,
                })
                .OrderByDescending(a => a.Nome)
                .ToList();

            return lst;
        }

        [HttpGet]
        [Route("api/GrupoUsuario/GetAllAtivos")]
        public IEnumerable<object> GetAllAtivos()
        {
            var lst = dbContext.GruposUsuarios
                .Where(a => !a.Deletado && a.Ativo)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Ativo,
                    a.Deletado,
                })
                .OrderByDescending(a => a.Nome)
                .ToList();

            return lst;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var grupo = dbContext.GruposUsuarios
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Ativo,
                    a.Deletado,
                })
                .FirstOrDefault();

            if (grupo == null || grupo.Deletado == true)
            {
                return NotFound();
            }

            return Ok(grupo);
        }

        [HttpPut]
        public object Update(GruposUsuarios grupoNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var grupoAntigo = dbContext.GruposUsuarios.FirstOrDefault(a => a.Id == grupoNovo.Id);

                dbContext.Entry(grupoAntigo).CurrentValues.SetValues(grupoNovo);
                dbContext.SaveChanges();

                tool.MontaLog("Grupos Usuarios", "Grupo Usuario Id: " + grupoNovo.Id + " Nome: " + grupoNovo.Nome + " foi editado com sucesso.", usuarioId, "EDITAR");
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
                categoriaId = grupoNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(GruposUsuarios grupo, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.GruposUsuarios.Add(grupo);
                    dbContext.SaveChanges();

                    tool.MontaLog("Grupos Usuarios", "Grupo Usuario Id: " + grupo.Id + " Nome: " + grupo.Nome + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");

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

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var grupo = dbContext.GruposUsuarios.Find(id);

            try
            {
                if (grupo == null)
                {
                    return NotFound();
                }

                grupo.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("Grupos Usuarios", "Grupo Usuario Id: " + grupo.Id + " Nome: " + grupo.Nome + " foi deletado com sucesso.", usuarioId, "DELETAR");
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
