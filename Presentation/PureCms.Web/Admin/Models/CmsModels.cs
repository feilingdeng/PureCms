using PureCms.Core.Domain.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PureCms.Web.Admin.Models
{
    public class SiteModel : BasePaged<SiteInfo>
    {
        public string Logo { get; set; }

        public string Name { get; set; }

        public string Theme { get; set; }

        public string Url { get; set; }

        public bool? IsEnabled { get; set; }
        public bool? IsDefault { get; set; }
    }

    public class EditSiteModel
    {
        public int SiteId { get; set; }
        public string Logo { get; set; }
        [Required]
        public string Name { get; set; }

        public string Theme { get; set; }

        public string Url { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsDefault { get; set; }

        public SelectList Themes { get; set; }
    }

    public class ChannelModel : BasePaged<ChannelInfo>
    {
        public int? ChannelId { get; set; }
        public int? SiteId { get; set; }
        public int? ParentChannelId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public string OpenTarget { get; set; }
        public int? Level { get; set; }
        public ContentType? ContentType { get; set; }
        public bool? IsShow { get; set; }

        public bool? IsEnabled { get; set; }

        public SelectList Sites { get; set; }
    }

    public class EditChannelModel
    {
        [Range(0, int.MaxValue)]
        public int? ChannelId { get; set; }
        public int SiteId { get; set; }
        public int? ParentChannelId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public string OpenTarget { get; set; }
        //public string Content { get; set; }

        [Range(0, int.MaxValue)]
        public int? DisplayOrder { get; set; }
        public int? Level { get; set; }
        public bool IsEnabled { get; set; }
        public ContentType ContentType { get; set; }
        public bool IsShow { get; set; }
    }
    public class ArticleCategoryModel :  BasePaged<ArticleCategoryInfo>
    {
        public int? ArticleCategoryId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }

        public int? ParentArticleCategoryId { get; set; }

        public int? Level { get; set; }
        public int? DisplayOrder { get; set; }
        public bool IsEnable { get; set; }
    }
    public class EditArticleCategoryModel
    {
        public int ArticleCategoryId { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }

        public int ParentArticleCategoryId { get; set; }

        public int Level { get; set; }
        public int DisplayOrder { get; set; }

        public bool HasChild { get; set; }
        public bool IsEnable { get; set; }
    }
    public class ArticleModel : BasePaged<ArticleInfo>
    {
        public int ChannelId { get; set; }
        public int? Status { get; set; }
        public int? CategoryId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
        public string ChannelName { get; set; }

        public bool? IsShow { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
    public class EditArticleModel
    {
        public long ArticleId { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        [Required]

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Author { get; set; }
        public string Url { get; set; }
        [AllowHtml]
        public string Content { get; set; }

        public bool IsShow { get; set; }
    }

    public class EditContentModel
    {
        [Required]
        public int ChannelId { get; set; }
        public string ChannelName { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}