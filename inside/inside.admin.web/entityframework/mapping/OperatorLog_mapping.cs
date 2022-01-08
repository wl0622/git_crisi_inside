using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class OperatorLog_mapping : EntityTypeConfiguration<OperatorLogModel>
    {
        public OperatorLog_mapping()
        {
            this.ToTable("OperatorLog");
            this.HasKey(a => a.id);
        }
    }
}