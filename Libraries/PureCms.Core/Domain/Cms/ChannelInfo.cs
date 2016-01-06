using PetaPoco;

namespace PureCms.Core.Domain.Cms
{
    [TableName("Channels")]
    [PrimaryKey("ChannelId", autoIncrement = true)]
    public class ChannelInfo : BaseEntity
    {
        public int ChannelId { get; set; }
        public int SiteId { get; set; }
        public int ParentChannelId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public string OpenTarget { get; set; }
        public int Level { get; set; }
        public ContentType ContentType { get; set; }
        public bool IsShow { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsEnabled { get; set; }
    }

    public enum ContentType
    {
        /// <summary>
        /// 单页内容
        /// </summary>
        Single = 0
        /// <summary>
        /// 文章列表
        /// </summary>
        ,
        List = 1
            /// <summary>
            /// 链接
            /// </summary>
            , Link = 2
    }
}
