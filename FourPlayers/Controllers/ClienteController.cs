using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class ClienteController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/Cliente/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.Clientes
                .Where(a => !a.Deletado)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    DataNascimento = a.DataNascimento.HasValue == true ? a.DataNascimento.Value.ToString("dd/MM/yyyy") : "",
                    a.Email,
                    a.PlanoId,
                    Plano = a.Planos.Nome,
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
        [Route("api/Cliente/GetAllAtivos")]
        public IEnumerable<object> GetAllAtivos()
        {
            var lst = dbContext.Clientes
                .Where(a => !a.Deletado && a.Ativo)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    DataNascimento = a.DataNascimento.HasValue == true ? a.DataNascimento.Value.ToString("dd/MM/yyyy") : "",
                    a.Email,
                    a.PlanoId,
                    Plano = a.Planos.Nome,
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
            var cliente = dbContext.Clientes
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    DataNascimento = a.DataNascimento.HasValue == true ? a.DataNascimento.Value.ToString("dd/MM/yyyy") : "",
                    a.Email,
                    a.PlanoId,
                    Plano = a.Planos.Nome,
                    a.Telefone,
                    a.Telefone2,
                    a.DataCadastro,
                    a.Ativo,
                    a.Deletado,
                })
                .FirstOrDefault();

            if (cliente == null || cliente.Deletado == true)
            {
                return NotFound();
            }

            return Ok(cliente);
        }

        [HttpPut]
        public object Update(Clientes clienteNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var clienteAntigo = dbContext.Clientes.FirstOrDefault(a => a.Id == clienteNovo.Id);

                clienteNovo.DataCadastro = clienteAntigo.DataCadastro;
                clienteNovo.Senha = clienteAntigo.Senha;
                clienteNovo.Hash = clienteAntigo.Hash;

                dbContext.Entry(clienteAntigo).CurrentValues.SetValues(clienteNovo);
                dbContext.SaveChanges();

                tool.MontaLog("Clientes", "Cliente Id: " + clienteNovo.Id + " Nome: " + clienteNovo.Nome + " foi editado com sucesso.", usuarioId, "EDITAR");
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
                clienteId = clienteNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(Clientes cliente, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    cliente.DataCadastro = DateTime.Now;
                    cliente.Senha = tool.Criptografar(cliente.Senha);
                    cliente.Hash = tool.Criptografar(cliente.DataCadastro + "-" + cliente.Id);

                    dbContext.Clientes.Add(cliente);
                    dbContext.SaveChanges();

                    tool.MontaLog("Clientes", "Cliente Id: " + cliente.Id + " Nome: " + cliente.Nome + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");

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
                categoriaId = cliente.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var cliente = dbContext.Clientes.Find(id);

            try
            {
                if (cliente == null || cliente.Deletado)
                {
                    return NotFound();
                }

                cliente.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("Clientes", "Cliente Id: " + cliente.Id + " Nome: " + cliente.Nome + " foi deletado com sucesso.", usuarioId, "DELETAR");
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
