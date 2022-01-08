
using inside.admin.web.entityframework.tableEntity;

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class t_article_list_mapping : EntityTypeConfiguration<t_article_list_model>
    {
        public t_article_list_mapping()
        {
            this.ToTable("Article");
            this.HasKey(a => a.articleID);
        }
    }
}