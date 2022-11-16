using System.Net;
using System.Net.Mail;

namespace INFOTOOLSSV.Data
{
    public class Email
    {
        public void Enviar(string correo, string token)
        {
            Correo(correo, token);
        }

        void Correo(string correo_receptor, string token)
        {
            string correo_emisor="tucorreo@hotmail.com";
            string clave_emisor="tuclavedeacceso";

            MailAddress receptor= new(correo_receptor);
            MailAddress emisor= new(correo_emisor);

            MailMessage email= new MailMessage(emisor, receptor);
            email.Subject="INFOTOOLSSV validacion de cuenta";
            email.Body="Para activar su cuenta, haga clic en el siguiente enlace: https://localhost:7146/Cuenta/Token?valor="+token;

            SmtpClient smtp= new();
            smtp.Host="smtp.office365.com";
            smtp.Port=587;
            smtp.Credentials= new NetworkCredential(correo_emisor, clave_emisor);
            smtp.DeliveryMethod=SmtpDeliveryMethod.Network;
            smtp.EnableSsl=true;

            try
            {
                smtp.Send(email);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}