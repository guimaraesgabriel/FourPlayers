using FourPlayers.Context;
using FourPlayers.Helpers;
using System;
using System.Linq;
using System.Web.Http;

namespace FourPlayers.Controllers
{
    public class LoginController : ApiController
    {
        private FourPlayersContext dbContext = new FourPlayersContext();
        private Tools tool = new Tools();

        [HttpPost]
        public IHttpActionResult Login(dynamic obj)
        {
            try
            {
                string email = obj.email;
                string senha = obj.senha;
                string senhaCript = tool.Criptografar(senha);

                var usuario = dbContext.Usuarios.FirstOrDefault(a => a.Email == email && a.Senha == senhaCript);

                if (usuario != null && usuario.Ativo && !usuario.Deletado)
                {
                    tool.MontaLog("Login", "O usuário " + usuario.Nome + " logou.", usuario.Id, "LOGIN");

                    return Json(new
                    {
                        success = true,
                        usuarioNome = usuario.Nome,
                    });
                }
                else if (usuario != null && !usuario.Ativo)
                {
                    return Json(new { success = false, msg = "Usuário Inativo." });
                }
                else if (usuario != null && usuario.Deletado)
                {
                    return Json(new { success = false, msg = "Usuário Deletado." });
                }
                else
                {
                    return Json(new { success = false, msg = "E-mail ou senha incorretos." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
                throw ex;
            }
        }
    }
}
