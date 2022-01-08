using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace crsri.cn.DbObject.mapping
{
    public class t_web_special_mapping: EntityTypeConfiguration<t_web_special_model>
    {
        public t_web_special_mapping()
        {
            this.ToTable("Special");
            this.HasKey(a => a.spID);
        }
    }
}
