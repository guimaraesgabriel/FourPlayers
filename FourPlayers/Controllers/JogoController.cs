using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class JogoController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        [Route("api/Jogo/GetAll")]
        public IEnumerable<object> GetAll()
        {
            var lst = dbContext.Jogos
                .Where(a => !a.Deletado)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.CapaURL,
                    a.Pontuacao,
                    a.Sinopse,
                    a.DataCadastro,
                    a.Ativo,
                    a.Deletado,
                })
                .OrderByDescending(a => a.Nome)
                .ToList();

            return lst;
        }

        [HttpGet]
        [Route("api/Jogo/GetAllAtivos")]
        public IEnumerable<object> GetAllAtivos()
        {
            var lst = dbContext.Jogos
                .Where(a => !a.Deletado && a.Ativo)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.CapaURL,
                    a.Pontuacao,
                    a.Sinopse,
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
            var jogo = dbContext.Jogos
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.CapaURL,
                    a.Pontuacao,
                    a.Sinopse,
                    a.DataCadastro,
                    a.Ativo,
                    a.Deletado,
                })
                .FirstOrDefault();

            if (jogo == null || jogo.Deletado == true)
            {
                return NotFound();
            }

            return Ok(jogo);
        }

        [HttpPut]
        public object Update(Jogos jogoNovo, int usuarioId)
        {
            var atualiza = true;
            var erro = string.Empty;

            try
            {
                var jogoAntigo = dbContext.Jogos.FirstOrDefault(a => a.Id == jogoNovo.Id);

                jogoNovo.DataCadastro = jogoAntigo.DataCadastro;

                dbContext.Entry(jogoAntigo).CurrentValues.SetValues(jogoNovo);
                dbContext.SaveChanges();

                tool.MontaLog("Jogos", "Jogo Id: " + jogoNovo.Id + " Nome: " + jogoNovo.Nome + " foi editado com sucesso.", usuarioId, "EDITAR");
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
                jogoId = jogoNovo.Id
            };

            return objRetorno;
        }

        [HttpPost]
        public object Save(Jogos jogo, int usuarioId)
        {
            var insere = true;
            var erro = string.Empty;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Jogos.Add(jogo);
                    dbContext.SaveChanges();

                    tool.MontaLog("Jogos", "Jogo Id: " + jogo.Id + " Nome: " + jogo.Nome + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");

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
                jogoId = jogo.Id
            };

            return objRetorno;
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            var jogo = dbContext.Jogos.Find(id);

            try
            {
                if (jogo == null)
                {
                    return NotFound();
                }

                jogo.Deletado = true;
                dbContext.SaveChanges();

                tool.MontaLog("Jogos", "Jogo Id: " + jogo.Id + " Nome: " + jogo.Nome + " foi deletado com sucesso.", usuarioId, "DELETAR");
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

        [HttpPost]
        [Route("api/Jogo/Upload")]
        public bool Upload(int id)
        {
            try
            {
                var jogo = dbContext.Jogos.FirstOrDefault(a => a.Id == id);

                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files["Imagem"];
                string folder = HttpContext.Current.Server.MapPath("/assets/img/jogos/" + jogo.Id + "/");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string hashName = String.Empty;

                using (var sha = new System.Security.Cryptography.SHA256Managed())
                {
                    byte[] textData = System.Text.Encoding.UTF8.GetBytes(postedFile.FileName);
                    byte[] hash = sha.ComputeHash(textData);

                    hashName = BitConverter.ToString(hash).Replace("-", String.Empty);
                }

                var tipo = postedFile.ContentType.Split('/')[1];
                string nomeArquivo = hashName + "." + tipo;

                var filePath = folder + nomeArquivo;
                postedFile.SaveAs(filePath);

                jogo.CapaURL = "jogos/" + jogo.Id + "/" + nomeArquivo.Replace("+", "");
                dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
    }
}
