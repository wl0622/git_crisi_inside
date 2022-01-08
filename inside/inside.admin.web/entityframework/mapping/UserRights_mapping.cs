using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class UserRights_mapping : EntityTypeConfiguration<UserRights_model>
    {
        public UserRights_mapping()
        {
            this.ToTable("UserRights");
            this.HasKey(a => a.id);
        }
    }
}