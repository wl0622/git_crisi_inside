using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace crsri.cn.DbObject.mapping
{
    public class t_admin_list_mapping : EntityTypeConfiguration<t_admin_list_model>
    {
        public t_admin_list_mapping()
        {
            this.ToTable("t_admin_list");
            this.HasKey(a => a.id);
        }
    }
}