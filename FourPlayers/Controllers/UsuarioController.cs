using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class UsuarioController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/usuario/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.Usuarios
                .Where(a => !a.Deletado)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    DataNascimento = a.DataNascimento.HasValue == true ? a.DataNascimento.Value.ToString("dd/MM/yyyy") : "",
                    a.Email,
                    a.GrupoUsuarioId,
                    GrupoUsuario = a.GruposUsuarios.Nome,
                    a.Telefone,
                    a.Telefone2,
                    a.DataCadastro,
                    a.Ativo,
                    a.Deletado,
                })
                .OrderByDescending(a => a.Nome)
                .ToList();

            return lst;
        }

        [HttpGet]
        [Route("api/usuario/GetAllAtivos")]
        public IEnumerable<object> GetAllAtivos()
        {
            var lst = dbContext.Usuarios
                .Where(a => !a.Deletado && a.Ativo)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    DataNascimento = a.DataNascimento.HasValue == true ? a.DataNascimento.Value.ToString("dd/MM/yyyy") : "",
                    a.Email,
                    a.GrupoUsuarioId,
                    GrupoUsuario = a.GruposUsuarios.Nome,
                    a.Telefone,
                    a.Telefone2,
                    a.DataCadastro,
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
            var usuario = dbContext.Usuarios
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    DataNascimento = a.DataNascimento.HasValue == true ? a.DataNascimento.Value.ToString("dd/MM/yyyy") : "",
                    a.Email,
                    a.GrupoUsuarioId,
                    GrupoUsuario = a.GruposUsuarios.Nome,
                    a.Telefone,
                    a.Telefone2,
                    a.DataCadastro,
                    a.Ativo,
                    a.Deletado,
                })
                .FirstOrDefault();

            if (usuario == null || usuario.Deletado == true)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPut]
        public object Update(Usuarios usuarioNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var usuarioAntigo = dbContext.Usuarios.FirstOrDefault(a => a.Id == usuarioNovo.Id);

                usuarioNovo.DataCadastro = usuarioAntigo.DataCadastro;
                usuarioNovo.Senha = usuarioAntigo.Senha;
                usuarioNovo.Hash = usuarioAntigo.Hash;

                dbContext.Entry(usuarioAntigo).CurrentValues.SetValues(usuarioNovo);
                dbContext.SaveChanges();

                tool.MontaLog("Usuarios", "usuario Id: " + usuarioNovo.Id + " Nome: " + usuarioNovo.Nome + " foi editado com sucesso.", usuarioId, "EDITAR");
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
                usuarioId = usuarioNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(Usuarios usuario, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    usuario.DataCadastro = DateTime.Now;
                    usuario.Senha = tool.Criptografar(usuario.Senha);
                    usuario.Hash = tool.Criptografar(usuario.DataCadastro + "-" + usuario.Id);

                    dbContext.Usuarios.Add(usuario);
                    dbContext.SaveChanges();

                    tool.MontaLog("Usuarios", "usuario Id: " + usuario.Id + " Nome: " + usuario.Nome + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");

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
                categoriaId = usuario.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var usuario = dbContext.Usuarios.Find(id);

            try
            {
                if (usuario == null || usuario.Deletado)
                {
                    return NotFound();
                }

                usuario.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("Usuarios", "usuario Id: " + usuario.Id + " Nome: " + usuario.Nome + " foi deletado com sucesso.", usuarioId, "DELETAR");
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
