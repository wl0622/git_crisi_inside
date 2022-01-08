using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace crsri.cn.DbObject.mapping
{
    public class t_web_qualManage_mapping: EntityTypeConfiguration<t_web_qualManage_model>
    {
        public t_web_qualManage_mapping()
        {
            this.ToTable("QualManage");
            this.HasKey(a => a.QMID);
        }
    }
}
