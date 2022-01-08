using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class chengguo_mapping : EntityTypeConfiguration<chengguoClass>
    {
        public chengguo_mapping()
        {
            this.ToTable("chengguo");
            this.HasKey(a => a.chengguoID);
        }
    }
}