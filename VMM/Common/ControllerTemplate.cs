using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections;
using AdoLib;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Security.Claims;

namespace VMM.Common {
    public class ControllerTemplate : Controller {
        public Database db;
        public string strEnv;
        public Hashtable aFields;

        public ControllerTemplate(string _strEnv = "test") {
            this.strEnv = _strEnv;
            this.aFields = new Hashtable();
            this.SetDatabase(this.strEnv);
        }

        public void SetDatabase(string _strEnv = "test") {
            this.strEnv = _strEnv;
            string strConn;

            switch (this.strEnv) {
                case "production":
                    strConn = ConfigurationManager.ConnectionStrings["JPROD"].ConnectionString;
                    break;

                default:
                    strConn = ConfigurationManager.ConnectionStrings["JPROD"].ConnectionString;
                    break;
            }

            db = new Database(strConn);
        }

        private void SetParameters() {

        }

        public void LogException(Exception ex) {
            string strMessage = ex.Message;
            Exception ex2 = ex;

            while (ex2.InnerException != null) {
                ex2 = ex.InnerException;
                strMessage += " \\ " + ex2.Message;
            }

            SqlCommand cmd = new SqlCommand("insert into systemerror(errortext, uri, userid) values(@error, @uri, @userid)");
            cmd.Parameters.AddWithValue("@error", strMessage);
            cmd.Parameters.AddWithValue("@uri", Request.Url.AbsoluteUri);
            if (Request.IsAuthenticated) {
                string strUserId = ((ClaimsIdentity)User.Identity).FindFirst("UserId").Value;
                cmd.Parameters.AddWithValue("@userid", strUserId);
            }
            db.ExecNonQuery(cmd);
        }
    }
}