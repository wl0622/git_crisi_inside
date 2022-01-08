
using inside.admin.web.model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class t_users_mapping : EntityTypeConfiguration<UsersModel>
    {
        public t_users_mapping()
        {
            this.ToTable("Users");
            this.HasKey(a => a.userID);
        }
    }
}