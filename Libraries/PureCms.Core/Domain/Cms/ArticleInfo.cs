using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Domain.Cms
{
    [TableName("Articles")]
    [PrimaryKey("ArticleId", autoIncrement = true)]
    public class ArticleInfo : BaseEntity
    {
        public long ArticleId { get; set; }
        public int CategoryId { get; set; }
        public int ChannelId { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Author { get; set; }
        public string Url { get; set; }

        public string Content { get; set; }

        public bool IsShow { get; set; }
        public int Status { get; set; }

        public DateTime? UpdatedOn { get; set; }

        [ResultColumn]
        [LinkEntity(typeof(ArticleCategoryInfo), SourceFieldName = "name")]
        public string CategoryName { get; set; }
    }
}
