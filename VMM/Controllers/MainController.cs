using VMM.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Net.Mail;
using System.Net;

namespace VMM.Controllers {
    public class MainController : ControllerTemplate {
        // GET: Main
        [AllowAnonymous]
        public ActionResult Index() {
            ViewBag.Page = "Victoria Marie";
            if(Request["action"] != null) {
                if (Request["firstname"] != null && Request["lastname"] != null && Request["email"] != null) {
                    string strFirstName = Request["firstname"].ToString();
                    string strLastName = Request["lastname"].ToString();
                    string strEmail = Request["email"].ToString();

                    SqlCommand cmd = new SqlCommand(@"select * from email where email=@email");
                    cmd.Parameters.AddWithValue("@email", strEmail);
                    XElement nd = db.ExecQueryElem(cmd);

                    if (nd != null) {
                        ViewBag.Snack = "This email has already been used.";
                    } else {
                        cmd = new SqlCommand("insert into email(firstname, lastname, email) values(@firstname, @lastname, @email)");
                        cmd.Parameters.AddWithValue("@firstname", strFirstName);
                        cmd.Parameters.AddWithValue("@lastname", strLastName);
                        cmd.Parameters.AddWithValue("@email", strEmail);
                        db.ExecNonQuery(cmd);
                        ViewBag.Snack = "Thanks for stopping by " + strFirstName +"!";
                        sendEmail(strFirstName, strLastName, strEmail);
                    }
                } else {
                    ViewBag.Snack = "Please fill in all valid fields";
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Error() {
            try {
                this.LogException(Server.GetLastError());
            } catch { }
            return View();
        }

        public void sendEmail(string strFirstName, string strLastName, string strEmail) {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
            client.Credentials = new NetworkCredential("jayrsawal", "viOs_6021!");
            MailMessage mail = new MailMessage("donotreply@victoriamariemusic.com", "jayrsawal@gmail.com");
            mail.Subject = strFirstName + " " + strLastName;
            mail.Body = strFirstName + " " + strLastName + " (" + strEmail + ")" ;
            try {
                client.Send(mail);
            } catch {
            }
        }
    }
}