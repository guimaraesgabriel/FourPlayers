using FourPlayers.Context;
using FourPlayers.Helpers;
using FourPlayers.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class JogoImagemController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpGet]
        public IEnumerable<object> GetAll(int jogoId)
        {
            var lst = dbContext.JogosImagens
                .Where(a => a.JogoId == jogoId)
                .Select(a => new
                {
                    a.Id,
                    a.JogoId,
                    Jogo = a.Jogos.Nome,
                    a.URL,
                })
                .ToList();

            return lst;
        }

        [HttpPost]
        public object Post(int jogoId, int usuarioId)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var httpRequest = HttpContext.Current.Request;
                    var jogo = dbContext.Jogos.FirstOrDefault(a => a.Id == jogoId);

                    string folder = HttpContext.Current.Server.MapPath("/assets/img/jogos/" + jogoId + "/");

                    if (httpRequest.Files.Count > 0)
                    {
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        foreach (string file in httpRequest.Files)
                        {
                            var postedFile = httpRequest.Files[file];
                            string hashName = String.Empty;

                            using (var sha = new System.Security.Cryptography.SHA256Managed())
                            {
                                byte[] textData = System.Text.Encoding.UTF8.GetBytes(postedFile.FileName);
                                byte[] hash = sha.ComputeHash(textData);

                                hashName = BitConverter.ToString(hash).Replace("-", String.Empty);
                            }

                            var tipo = postedFile.ContentType.Split('/')[1];
                            string nomeArquivo = hashName + "." + tipo;

                            var filePath = folder + nomeArquivo.Replace("+", "");
                            postedFile.SaveAs(filePath);

                            var jogoImagem = new JogosImagens();
                            jogoImagem.URL = "jogos/" + jogoId + "/" + nomeArquivo.Replace("+", "");
                            jogoImagem.JogoId = jogoId;

                            dbContext.JogosImagens.Add(jogoImagem);
                            dbContext.SaveChanges();

                            tool.MontaLog("JogosImagens", "Jogo Imagem Id: " + jogoImagem.Id + " Jogo: " + jogoImagem.Jogos.Nome + " Imagem: " + jogoImagem.URL + " foi adicionado com sucesso.", usuarioId, "ADICIONAR");
                        }

                        transaction.Commit();
                    }

                    var obj = new
                    {
                        statusReq = true,
                        erro = ""
                    };

                    return obj;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    var obj = new
                    {
                        statusReq = false,
                        erro = ex.Message
                    };

                    return obj;
                }
            }
        }

        [HttpDelete]
        public object Delete(int id, int usuarioId)
        {
            var delete = true;
            var erro = string.Empty;

            try
            {
                var jogoImagem = dbContext.JogosImagens.Find(id);

                if (jogoImagem != null)
                {
                    var jogoImagemId = jogoImagem.Id;
                    var jogo = jogoImagem.Jogos.Nome;
                    var url = jogoImagem.URL;

                    dbContext.JogosImagens.Remove(jogoImagem);
                    dbContext.SaveChanges();

                    tool.MontaLog("JogosImagens", "Jogo Imagem Id: " + jogoImagemId + " Jogo: " + jogo + " Imagem: " + url + " foi deletado com sucesso.", usuarioId, "DELETAR");
                }
                else
                {
                    return NotFound();
                }
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
