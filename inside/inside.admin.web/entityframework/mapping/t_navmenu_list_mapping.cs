using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class t_navmenu_list_mapping : EntityTypeConfiguration<t_navmenu_list_model>
    {
        public t_navmenu_list_mapping()
        {
            this.ToTable("t_navMenu");
            this.HasKey(a => a.id);
        }
    }
}