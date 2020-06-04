using FourPlayers.Context;
using FourPlayers.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace FourPlayers.Helpers
{
    public class Tools
    {
        private FourPlayersContext dbContext = new FourPlayersContext();

        public string Criptografar(string text)
        {
            try
            {
                byte[] buffer = Encoding.Default.GetBytes(text);
                SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
                string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

                return hash;
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }

        public void MontaLog(string tela, string descricao, int usuarioId, string acao)
        {
            Log l = new Log();
            l.Data = DateTime.Now;
            l.Tela = tela;
            l.Descricao = descricao;
            l.UsuarioId = usuarioId;
            l.Acao = acao;

            try
            {
                dbContext.Log.Add(l);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                l = new Log();
                l.Data = DateTime.Now;
                l.Tela = "ERRO Log";
                l.Descricao = "ERRO AO SALVAR Log " + ex.Message;
                l.UsuarioId = usuarioId;
                l.Acao = "ERRO";

                dbContext.Log.Add(l);
                dbContext.SaveChanges();
            }
        }
    }
}