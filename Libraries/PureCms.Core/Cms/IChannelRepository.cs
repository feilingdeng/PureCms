using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface IChannelRepository
    {
        int Create(ChannelInfo entity);

        bool Update(ChannelInfo entity);
        bool Update(UpdateContext<ChannelInfo> q);

        bool DeleteById(int id);

        long Count(QueryDescriptor<ChannelInfo> q);
        PagedList<ChannelInfo> Query(QueryDescriptor<ChannelInfo> q);
        List<ChannelInfo> GetAll(QueryDescriptor<ChannelInfo> q);

        ChannelInfo GetById(int id);
        int MoveNode(int moveid, int targetid, int parentid, string position);
    }
}
