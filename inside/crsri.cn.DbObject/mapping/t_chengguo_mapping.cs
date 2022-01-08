using crsri.cn.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace crsri.cn.DbObject.mapping
{
    public class t_chengguo_mapping: EntityTypeConfiguration<chengguoClass>
    {
        public t_chengguo_mapping()
        {
            this.ToTable("chengguo");
            this.HasKey(a => a.chengguoID);
        }
    }
}
