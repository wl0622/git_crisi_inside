
using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class errorLoginRecord_mapping : EntityTypeConfiguration<errorLoginRecordModel>
    {
        public errorLoginRecord_mapping()
        {
            this.ToTable("t_errorLoginRecord");
            this.HasKey(a => a.id);
        }
    }
}