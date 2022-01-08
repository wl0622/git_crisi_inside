
using inside.admin.web.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inside.admin.web.entityframework.reqmodel
{
    public class reqUserSessionInfoModel
    {
        public UsersModel uInfo { get; set; }
        public string uRightsID { get; set; }
    }
}