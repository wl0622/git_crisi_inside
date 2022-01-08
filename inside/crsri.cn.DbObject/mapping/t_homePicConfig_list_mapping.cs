using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace crsri.cn.DbObject.mapping
{
    public class t_homePicConfig_list_mapping : EntityTypeConfiguration<t_homePicConfig_list_model>
    {
        public t_homePicConfig_list_mapping()
        {
            this.ToTable("t_homePicConfig");
            this.HasKey(a => a.id);
        }
    }
}
