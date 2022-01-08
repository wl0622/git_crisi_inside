using inside.admin.web.entityframework.tableEntity;

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.mapping
{
    public class t_special_list_mapping: EntityTypeConfiguration<t_special_list_model>
    {
        public t_special_list_mapping()
        {
            this.ToTable("Special");
            this.HasKey(a => a.spID);
        }
    }


        
    }
