using inside.admin.web.entityframework.tableEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class UserGroups_mapping: EntityTypeConfiguration<UserGroups_model>
    {
        public UserGroups_mapping()
        {
            this.ToTable("UserGroups");
            this.HasKey(a => a.userGroupID);
        }
    }
}