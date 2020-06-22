using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class MovimentacaoFinanceiraController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.MovimentacoesFinanceiras
                .Where(a => !a.Deletado)
                .Select(a => new
                {
                    a.Id,
                    a.Data,
                    a.Descricao,
                    a.Tipo,
                    a.UsuarioId,
                    Usuario = a.Usuarios.Nome,
                    a.Valor,
                })
                .OrderByDescending(a => a.Data)
                .ToList();

            return lst;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var financeiro = dbContext.MovimentacoesFinanceiras
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Data,
                    a.Descricao,
                    a.Tipo,
                    a.UsuarioId,
                    Usuario = a.Usuarios.Nome,
                    a.Valor,
                    a.Deletado,
                })
                .FirstOrDefault();

            if (financeiro == null || financeiro.Deletado)
            {
                return NotFound();
            }

            return Ok(financeiro);
        }

        [HttpPut]
        public object Update(MovimentacoesFinanceiras financeiroNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var financeiroAntigo = dbContext.MovimentacoesFinanceiras.FirstOrDefault(a => a.Id == financeiroNovo.Id);

                financeiroNovo.Data = financeiroAntigo.Data;

                dbContext.Entry(financeiroAntigo).CurrentValues.SetValues(financeiroNovo);
                dbContext.SaveChanges();

                tool.MontaLog("MovimentacoesFinanceiras", "Movimentação Financeira Id: " + financeiroNovo.Id + " Descrição: " + financeiroNovo.Descricao + " foi editado com sucesso.", usuarioId, "EDITAR");
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
                financeiroId = financeiroNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(MovimentacoesFinanceiras financeiro, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.MovimentacoesFinanceiras.Add(financeiro);
                    dbContext.SaveChanges();

                    tool.MontaLog("MovimentacoesFinanceiras", "Movimentação Financeira Id: " + financeiro.Id + " Descrição: " + financeiro.Descricao + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");

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
                financeiroId = financeiro.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var financeiro = dbContext.MovimentacoesFinanceiras.Find(id);

            try
            {
                if (financeiro == null)
                {
                    return NotFound();
                }

                financeiro.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("MovimentacoesFinanceiras", "Movimentação Financeira Id: " + financeiro.Id + " Descrição: " + financeiro.Descricao + " foi deletado com sucesso.", usuarioId, "DELETAR");
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
