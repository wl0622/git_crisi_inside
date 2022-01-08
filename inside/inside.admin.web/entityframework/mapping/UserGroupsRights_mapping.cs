using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class UserGroupsRights_mapping: EntityTypeConfiguration<UserGroupsRights_model>
    {
        public UserGroupsRights_mapping()
        {
            this.ToTable("UserGroupsRights");
            this.HasKey(a => a.id);
        }
    }
}