using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class loginLog_mapping : EntityTypeConfiguration<loginLogModel>
    {
        public loginLog_mapping()
        {
            this.ToTable("t_loginLog");
            this.HasKey(a => a.id);
        }
    }
}