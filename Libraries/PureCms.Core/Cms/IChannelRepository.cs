using PureCms.Core.Context;
using PureCms.Core.Domain.Cms;
using System.Collections.Generic;

namespace PureCms.Core.Cms
{
    public interface IChannelRepository
    {
        int Create(ChannelInfo entity);

        bool Update(ChannelInfo entity);
        bool Update(List<KeyValuePair<string, object>> sets, ChannelQueryContext q);

        bool DeleteById(int id);

        long Count(ChannelQueryContext q);
        PagedList<ChannelInfo> Query(ChannelQueryContext q);
        List<ChannelInfo> GetAll(ChannelQueryContext q);

        ChannelInfo GetById(int id);
        int MoveNode(int moveid, int targetid, int parentid, string position);
    }
}
