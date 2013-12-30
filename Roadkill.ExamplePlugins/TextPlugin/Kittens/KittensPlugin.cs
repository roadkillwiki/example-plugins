using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;

// From https://github.com/Workshop2/RoadKill.Plugins.Kittens
namespace Roadkill.ExamplePlugins.TextPlugin
{
    public class KittensPlugin : Roadkill.Core.Plugins.TextPlugin
    {
        public override string Id
        {
            get { return "KittensPlugin"; }
        }

        public override string Name
        {
            get { return "KittensPlugin"; }
        }

        public override string Description
        {
            get { return "KittensPlugin"; }
        }

        public override string Version
        {
            get { return "1.0.0"; }
        }

        public override string AfterParse(string html)
        {
            const string placeKitten = "http://placekitten.com/g/{0}/{1}";
            const string regex = @"<img\s*src=""(?<src>.+?)""";
            const RegexOptions myRegexOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
            Regex myRegex = new Regex(regex, myRegexOptions);

            foreach (Match myMatch in myRegex.Matches(html))
            {
                if (myMatch.Success)
                {
                    Group group = myMatch.Groups["src"];
                    string fileName = GetFileName(group.Value);

                    string path = Path.Combine(ApplicationSettings.AttachmentsDirectoryPath, fileName);

                    ImageInfo imageSize = GetImageSize(path);
                    string url = string.Format(placeKitten, imageSize.Width, imageSize.Height);

                    html = html.Replace(group.Value, url);
                }
            }

            return html;
        }


        public string GetFileName(string fileName)
        {
            fileName = fileName.Replace("&#x2E;", ".");
            fileName = fileName.Replace("&#x2F;", "/");

            if (fileName.StartsWith("/Attachments/"))
            {
                fileName = fileName.Substring("/Attachments/".Length);
            }

            return fileName;
        }

        private static readonly Dictionary<string, ImageInfo> ImageInfo = new Dictionary<string, ImageInfo>();
        private static ImageInfo GetImageSize(string path)
        {
            ImageInfo result;

            if (ImageInfo.ContainsKey(path.ToLower()))
            {
                result = ImageInfo[path.ToLower()];
            }
            else
            {
                result = new ImageInfo();

                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (Image image = Image.FromStream(file, useEmbeddedColorManagement: false, validateImageData: false))
                    {
                        result.Width = (int)image.PhysicalDimension.Width;
                        result.Height = (int)image.PhysicalDimension.Height;
                    }
                }

                ImageInfo.Add(path.ToLower(), result);
            }

            return result;
        }
    }
}