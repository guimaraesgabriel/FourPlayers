using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class CategoriaController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/Categoria/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.Categorias
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
            var categoria = dbContext.Categorias
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Deletado
                })
                .FirstOrDefault();

            if (categoria == null || categoria.Deletado == true)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPut]
        public object Update(Categorias categoriaNova, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var categoriaAntiga = dbContext.Categorias.FirstOrDefault(a => a.Id == categoriaNova.Id);

                dbContext.Entry(categoriaAntiga).CurrentValues.SetValues(categoriaNova);
                dbContext.SaveChanges();

                tool.MontaLog("Categorias", "Categoria Id: " + categoriaNova.Id + " Nome: " + categoriaNova.Nome + " foi editada com sucesso.", usuarioId, "EDITAR");
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
                categoriaId = categoriaNova.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(Categorias categoria, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Categorias.Add(categoria);
                    dbContext.SaveChanges();

                    tool.MontaLog("Categorias", "Categoria Id: " + categoria.Id + " Nome: " + categoria.Nome + " foi adicionada com sucesso.", usuarioId, "ADICIONAR");

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
                categoriaId = categoria.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var categoria = dbContext.Categorias.Find(id);

            try
            {
                if (categoria == null || categoria.Deletado)
                {
                    return NotFound();
                }

                categoria.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("Categorias", "Categoria Id: " + categoria.Id + " Nome: " + categoria.Nome + " foi deletada com sucesso.", usuarioId, "DELETAR");
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
