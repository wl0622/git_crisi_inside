using inside.admin.web.entityframework;
using inside.admin.web.helper;
using inside.admin.web.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace inside.admin.web.ashx
{
    /// <summary>
    /// Summary description for excelUpload
    /// </summary>
    public class excelUpload : IHttpHandler
    {
        EFHELP efhelp = new EFHELP();
        public void ProcessRequest(HttpContext context)
        {
            jsonMessageModel jm = new jsonMessageModel();
            jm.status = "ok";
            var request = context.Request;
            if (request["method"] != null)
            {
                string method = request["method"];
                if (method == "score")
                {
                    DataTable excelTable = new DataTable();

                    HttpPostedFile mypost = context.Request.Files[0];
                    string fileName = context.Request.Files[0].FileName;
                    string serverpath = context.Server.MapPath(string.Format("~/{0}", "scroeExcel"));
                    string path = System.IO.Path.Combine(serverpath, DateTime.Now.ToString("yyyyMMddhhmmss") + "_" + fileName);
                    mypost.SaveAs(path);
            
                    excelTable = ImportExcelHelper.GetExcelDataTable(path);
                    DataColumn dc = new DataColumn("ksfsm");
                    excelTable.Columns.Add(dc);
                    excelTable.Columns["ksbh"].SetOrdinal(0);
                    excelTable.Columns["xm"].SetOrdinal(1);
                    excelTable.Columns["ksfsm"].SetOrdinal(2);
                    excelTable.Columns["zzll"].SetOrdinal(3);
                    excelTable.Columns["wgy"].SetOrdinal(4);
                    excelTable.Columns["ywk1"].SetOrdinal(5);
                    excelTable.Columns["ywk2"].SetOrdinal(6);
                    excelTable.Columns["zf"].SetOrdinal(7);  
                    efhelp.BulkCopy(excelTable, "PostGraduate");

                    jm.message = JsonConvert.SerializeObject(excelTable);
                    context.Response.Write(JsonConvert.SerializeObject(jm));

                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}