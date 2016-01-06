using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PureCms.Core;
using PureCms.Core.Utilities;
using PureCms.Utilities;
using PureCms.Services.Configuration;

namespace PureCms.Services.Common
{
    public class Uploader
    {
        private UploadSetting config = new SettingService().GetUploadSetting();
        /// <summary>
        /// 保存商品图片
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public string SaveUplaodProductImage(HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";


            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, config.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > config.UploadImgSize)
                return "-3";

            string dirPath = CommonHelper.MapPath("/upload/product/");
            string name = "ps_" + DateTime.Now.ToString("yyMMddHHmmssfffffff");
            string newFileName = name + extension;
            string[] sizeList = config.ProductShowThumbSize.SplitSafe(",");

            string sourceDirPath = string.Format("{0}source/", dirPath);
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);
            string sourcePath = sourceDirPath + newFileName;
            image.SaveAs(sourcePath);

            if (config.WatermarkType == 1)//文字水印
            {
                string path = string.Format("{0}{1}_text{2}", sourceDirPath, name, extension);
                IOHelper.GenerateTextWatermark(sourcePath, path, config.WatermarkText, config.WatermarkTextSize, config.WatermarkTextFont, config.WatermarkPosition, config.WatermarkQuality);
                sourcePath = path;
            }
            else if (config.WatermarkType == 2)//图片水印
            {
                string path = string.Format("{0}{1}_img{2}", sourceDirPath, name, extension);
                string watermarkPath = CommonHelper.MapPath("/watermarks/" + config.WatermarkImg);
                IOHelper.GenerateImageWatermark(sourcePath, watermarkPath, path, config.WatermarkPosition, config.WatermarkImgOpacity, config.WatermarkQuality);
                sourcePath = path;
            }

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = size.SplitSafe("_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       widthAndHeight[0].ToInt(),
                                       widthAndHeight[1].ToInt(),
                                       "H");
            }
            return newFileName;
        }
        /// <summary>
        /// 保存文章图片
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public string SaveUplaodArticleImage(HttpPostedFileBase image)
        {
            if (image == null)
                return "-1";


            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);
            if (!ValidateHelper.IsImgFileName(fileName) || !CommonHelper.IsInArray(extension, config.UploadImgType))
                return "-2";

            int fileSize = image.ContentLength;
            if (fileSize > config.UploadImgSize)
                return "-3";

            string dirPath = CommonHelper.MapPath("/upload/cms/");
            string name = "ps_" + DateTime.Now.ToString("yyMMddHHmmssfffffff");
            string newFileName = name + extension;
            string[] sizeList = config.ArticleThumbSize.SplitSafe(",");

            string sourceDirPath = string.Format("{0}source/", dirPath);
            if (!Directory.Exists(sourceDirPath))
                Directory.CreateDirectory(sourceDirPath);
            string sourcePath = sourceDirPath + newFileName;
            image.SaveAs(sourcePath);

            if (config.WatermarkType == 1)//文字水印
            {
                string path = string.Format("{0}{1}_text{2}", sourceDirPath, name, extension);
                IOHelper.GenerateTextWatermark(sourcePath, path, config.WatermarkText, config.WatermarkTextSize, config.WatermarkTextFont, config.WatermarkPosition, config.WatermarkQuality);
                sourcePath = path;
            }
            else if (config.WatermarkType == 2)//图片水印
            {
                string path = string.Format("{0}{1}_img{2}", sourceDirPath, name, extension);
                string watermarkPath = CommonHelper.MapPath("/watermarks/" + config.WatermarkImg);
                IOHelper.GenerateImageWatermark(sourcePath, watermarkPath, path, config.WatermarkPosition, config.WatermarkImgOpacity, config.WatermarkQuality);
                sourcePath = path;
            }

            foreach (string size in sizeList)
            {
                string thumbDirPath = string.Format("{0}thumb{1}/", dirPath, size);
                if (!Directory.Exists(thumbDirPath))
                    Directory.CreateDirectory(thumbDirPath);
                string[] widthAndHeight = size.SplitSafe("_");
                IOHelper.GenerateThumb(sourcePath,
                                       thumbDirPath + newFileName,
                                       widthAndHeight[0].ToInt(),
                                       widthAndHeight[1].ToInt(),
                                       "H");
            }
            return newFileName;
        }
    }
}
