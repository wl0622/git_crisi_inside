using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace inside.crsri.Dal
{
    public class downloadDal : baseDal
    {
        public static List<reqDownload> reqDownItems(string subjectID)
        {

            List<reqDownload> req = new List<reqDownload>();
            List<SqlParameter> para = new List<SqlParameter>();

            string findId = subjectID != null ? "a.subjectID like @subjectID+'%' and " : null;

            string sql = string.Format(@"select a.*,b.subjectName from dbo.DownService as a join [Subject] as b on a.subjectID=b.subjectID where {0} isDeleted=0 and isPassed=1 ", findId);

            if (findId != null)
            {
                para.Add(new SqlParameter() { ParameterName = "@subjectID", Value = subjectID });
            }
 


            try
            {

                DataTable dt = CrsriEntityFramework.QueryDataTable(sql, para.ToArray());
                IEnumerable<IGrouping<string, DataRow>> result = dt.Rows.Cast<DataRow>().GroupBy<DataRow, string>(dr => dr["subjectID"].ToString());


                foreach (IGrouping<string, DataRow> ig in result)
                {
                    reqDownload items = new reqDownload();
                    items.downloadItems = new List<t_web_downService_model>();
                    items.subjectID = ig.First()["subjectID"].ToString(); ;
                    items.subjectName = ig.First()["subjectName"].ToString();

                    foreach (DataRow dr in ig)
                    {
                        items.downloadItems.Add(new t_web_downService_model()
                        {
                            subjectID = dr["subjectID"].ToString(),
                            author = dr["author"].ToString(),
                            content = dr["content"].ToString(),
                            DSID = Convert.ToInt32(dr["DSID"]),
                            DSName = dr["DSName"].ToString(),
                            editor = dr["editor"].ToString(),
                            editorDep = dr["editorDep"].ToString(),
                            keywords = dr["keywords"].ToString(),
                            releaseDep = dr["releaseDep"].ToString(),
                            releaseTime = Convert.ToDateTime(dr["releaseTime"]),
                            updateTime = Convert.ToDateTime(dr["updateTime"]),
                            specialID = dr["specialID"].ToString()
                        });
                    }

                    req.Add(items);
                }
            }
            catch
            {

            }

            return req;

        }


        public static List<t_web_subject_model> reqSubjectItem()
        {

            return CrsriEntityFramework.QueryDataTable<t_web_subject_model>("select * from [Subject] where subjectID like '009%' and len(subjectID)=6");
        }


        public static List<t_web_downService_model> reqServiceContent(int DSID)
        {
            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter() { ParameterName = "@DSID", Value = DSID });
            return CrsriEntityFramework.QueryList<t_web_downService_model>("SELECT * FROM DownService WHERE DSID=@DSID", para.ToArray());
        }
    }
}