using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class zhuanjia_mapping : EntityTypeConfiguration<t_web_zhuanjia_model>
    {
        public zhuanjia_mapping()
        {
            this.ToTable("zhuanjia");
            this.HasKey(a => a.zhuanjiaID);
        }
    }
}