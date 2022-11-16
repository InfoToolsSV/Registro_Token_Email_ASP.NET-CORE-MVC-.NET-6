using Microsoft.AspNetCore.Mvc;
using INFOTOOLSSV.Data;
using INFOTOOLSSV.Models;
using System.Data;
using System.Data.SqlClient;

namespace INFOTOOLSSV.Controllers

{
    public class CuentaController : Controller
    {
        private readonly DbContext _contexto;

        public CuentaController(DbContext contento)
        {
            _contexto = contento;
        }

        public ActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrarse(UsuarioModel u)
        {
            try
            {
                using (SqlConnection con = new(_contexto.Valor))
                {
                    using (SqlCommand cmd = new("sp_registrar", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = u.Nombre;
                        cmd.Parameters.Add("@Apellido", SqlDbType.VarChar).Value = u.Apellido;
                        cmd.Parameters.Add("@Fecha_Nacimiento", SqlDbType.Date).Value = u.Fecha_Nacimiento;
                        cmd.Parameters.Add("@Correo", SqlDbType.VarChar).Value = u.Correo;
                        cmd.Parameters.Add("@Usuario", SqlDbType.VarChar).Value = u.User;
                        cmd.Parameters.Add("@Pass", SqlDbType.VarChar).Value = u.Password;
                        var token = Guid.NewGuid();
                        cmd.Parameters.Add("@Token", SqlDbType.VarChar).Value = token.ToString();
                        cmd.Parameters.Add("@Estado", SqlDbType.Bit).Value = 0;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        Email email = new();
                        if (u.Correo != null)
                            email.Enviar(u.Correo, token.ToString());

                        con.Close();
                    }
                }
                return RedirectToAction("Token", "Cuenta");

            }
            catch (System.Exception e)
            {
                ViewData["error"] = e.Message;
                return View();
            }
            //return View();
        }

        public ActionResult Token()
        {
            string token = Request.Query["valor"];

            if (token != null)
            {
                try
                {
                    using (SqlConnection con = new(_contexto.Valor))
                    {
                        using (SqlCommand cmd = new("sp_validar", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@Token", SqlDbType.VarChar).Value = token;

                            con.Open();
                            cmd.ExecuteNonQuery();

                            ViewData["mensaje"] = "Su cuenta ha sido validada exitosamente!";
                            con.Close();

                        }
                    }
                    return View();
                }
                catch (System.Exception e)
                {
                    ViewData["mensaje"] = e.Message;
                    return View();
                }
            }
            else
            {
                ViewData["mensaje"] = "Verifique su correo para activar su cuenta.";
                return View();
            }
        }
    }
}