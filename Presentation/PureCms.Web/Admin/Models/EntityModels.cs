using PureCms.Core.Components.Grid;
using PureCms.Core.Domain;
using PureCms.Core.Domain.Schema;
using PureCms.Core.Domain.Schema;
using System;
using System.Collections.Generic;

namespace PureCms.Web.Admin.Models
{
    public class EntityGridModel : BasePaged<dynamic>
    {
        public Guid? EntityId { get; set; }
        public Guid QueryId { get; set; }
        public GridInfo Grid { get; set; }
        public QueryViewInfo QueryView { get; set; }

        public List<EntityInfo> EntityList { get; set; }
        public List<AttributeInfo> AttributeList { get; set; }
    }
}