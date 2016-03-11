using PureCms.Core.Components.Grid;
using PureCms.Core.Domain;
using PureCms.Core.Domain.Query;
using System;

namespace PureCms.Web.Admin.Models
{
    public class EntityGridModel : BasePaged<dynamic>
    {
        public Guid? EntityId { get; set; }
        public Guid QueryId { get; set; }
        public GridInfo Grid { get; set; }
        public QueryViewInfo QueryView { get; set; }
    }
}