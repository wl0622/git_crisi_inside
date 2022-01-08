using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace crsri.cn.DbObject.mapping
{
    public class t_web_article_mapping : EntityTypeConfiguration<t_web_article_model>
    {
        public t_web_article_mapping()
        {
            this.ToTable("article");
            this.HasKey(a => a.articleID);
        }
    }
}
