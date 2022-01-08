using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace crsri.cn.DbObject.mapping
{
    public class t_web_siteCfgPic_mapping: EntityTypeConfiguration<t_web_siteCfgPic_model>
    {
        public t_web_siteCfgPic_mapping()
        {
            this.ToTable("siteCfgPic");
            this.HasKey(a => a.id);
        }
    }
}
