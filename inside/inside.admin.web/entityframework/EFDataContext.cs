using inside.admin.web.entityframework.mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework
{
    public class EFDataContext : DbContext
    {
        private bool isNew = true; //是否是新的sql执行  
        private string strMsg = ""; //sql执行的相关信息  
        private string strConn = ""; //数据库连接字符串  
        private string UserName = ""; //日志用户名称  
        private string AdditionalInfo = ""; //日志额外信息  


        public EFDataContext(string connString) : // 数据库链接字符串  
            base(connString)
        {
            strConn = connString;
            //DbConfiguration.SetConfiguration(new Configuration(connString, true));
            Database.SetInitializer<EFDataContext>(null); //设置为空，防止自动检查和生成
            base.Database.Log = (info) => Debug.WriteLine(info);
            this.Configuration.LazyLoadingEnabled = true;

            //布署时报错加上此句
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public EFDataContext(string connString, string logUserName, string logAdditionalInfo) : // 数据库链接字符串  
            base(connString)
        {
            strConn = connString;
            Database.SetInitializer<EFDataContext>(null); //设置为空，防止自动检查和生成  
            UserName = logUserName;
            AdditionalInfo = logAdditionalInfo;
            base.Database.Log = AddLogger;
            this.Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //去掉复数映射 
            #region 这里映射数据表关系
            modelBuilder.Configurations.Add(new t_article_list_mapping());
            modelBuilder.Configurations.Add(new t_users_mapping());
            modelBuilder.Configurations.Add(new loginLog_mapping());
            modelBuilder.Configurations.Add(new errorLoginRecord_mapping());
            modelBuilder.Configurations.Add(new t_subject_list_mapping());
            modelBuilder.Configurations.Add(new t_special_list_mapping());
            modelBuilder.Configurations.Add(new t_department_list_mapping());
            modelBuilder.Configurations.Add(new t_homePicConfig_list_mapping());
            modelBuilder.Configurations.Add(new t_navmenu_list_mapping());
            modelBuilder.Configurations.Add(new UserGroups_mapping());
            modelBuilder.Configurations.Add(new chengguo_mapping());
            modelBuilder.Configurations.Add(new UserGroupsRights_mapping());
            modelBuilder.Configurations.Add(new UserRights_mapping());
            modelBuilder.Configurations.Add(new OperatorLog_mapping());
            modelBuilder.Configurations.Add(new zhuanjia_mapping());
            
            #endregion
        }

        /// <summary>  
        /// 添加日志  
        /// </summary>  
        /// <param name="info"></param>  
        public void AddLogger(string info)
        {
            if (info != "\r\n" && (!info.Contains("Sys_EventLog")))
            {
                string strTemp = info.ToUpper().Trim();
                if (isNew)
                {
                    //记录增删改  
                    if (strTemp.StartsWith("INSERT") || strTemp.StartsWith("UPDATE") || strTemp.StartsWith("DELETE"))
                    {
                        strMsg = info;
                        isNew = false;
                    }
                }
                else
                {
                    if (strTemp.StartsWith("CLOSED CONNECTION"))
                    {
                        //增加新日志  
                        using (EFDataContext db = new EFDataContext(strConn))
                        {
                            try
                            {
                                //保存日志到数据库或其他地方  

                            }
                            catch (Exception ex)
                            {
                                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "//logError.txt"))
                                {
                                    sw.Write(ex.Message);
                                    sw.Flush();
                                }
                            }
                        }
                        //清空  
                        strMsg = "";
                        isNew = true;
                    }
                    else
                    {
                        strMsg += info;
                    }
                }

            }
        }
    }







    public class EFDataContext<T> : EFDataContext where T : class
    {

        public EFDataContext(string connString) : // 数据库链接字符串  
            base(connString)
        {
            Database.SetInitializer<EFDataContext<T>>(null);//设置为空，防止自动检查和生成 
            this.Configuration.LazyLoadingEnabled = true;
        }


        public EFDataContext(string connString, string logUserName, string logAdditionalInfo) : // 数据库链接字符串  
            base(connString, logUserName, logAdditionalInfo)
        {
            Database.SetInitializer<EFDataContext<T>>(null);//设置为空，防止自动检查和生成  
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<T> Entities { get; set; }
    }
}