using OctopusLibrary.Utility;
using System.Configuration;

namespace Parkheesung.WebUI.Models
{
    public class MailSender : MailHandler
    {
        public MailSender() : base()
        {
            string emailID = ConfigurationManager.AppSettings["GMailID"];
            string emailPassword = ConfigurationManager.AppSettings["GMailPWD"];

            this.HostAddress = "smtp.gmail.com";
            this.HostPort = 587;
            this.Initialize(emailID, emailPassword);
        }
    }
}