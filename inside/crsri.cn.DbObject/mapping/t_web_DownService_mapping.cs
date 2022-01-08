using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace crsri.cn.DbObject.mapping
{
    public class t_web_DownService_mapping: EntityTypeConfiguration<t_web_downService_model>
    {
        public t_web_DownService_mapping()
        {
            this.ToTable("DownService");
            this.HasKey(a => a.DSID);
        }
    }
}
