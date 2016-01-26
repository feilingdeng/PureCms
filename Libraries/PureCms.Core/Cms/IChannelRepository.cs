using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface IChannelRepository
    {
        int Create(ChannelInfo entity);

        bool Update(ChannelInfo entity);
        bool Update(UpdateContext<ChannelInfo> context);

        bool DeleteById(int id);

        long Count(QueryDescriptor<ChannelInfo> q);
        PagedList<ChannelInfo> QueryPaged(QueryDescriptor<ChannelInfo> q);
        List<ChannelInfo> Query(QueryDescriptor<ChannelInfo> q);

        ChannelInfo FindById(int id);
        int MoveNode(int moveid, int targetid, int parentid, string position);
    }
}
