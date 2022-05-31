using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace UvAssetList
{
    public class Program
    {
        public static void Main(String[] args)
        {
            // Find the *.csproj file to examine, either by argument or by looking in the current directory.
            var projFilePath = (args.Length > 0) ? args[0] : null;
            if (projFilePath == null)
            {
                var candidates = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj", SearchOption.TopDirectoryOnly);
                if (candidates.Length == 0)
                {
                    Console.WriteLine("No *.csproj files were found in the current directory.");
                    return;
                }
                if (candidates.Length > 1)
                {
                    Console.WriteLine("Multiple *.csproj files exist in this directory; please specify one to process.");
                    return;
                }
                projFilePath = candidates[0];                    
            }

            // Make sure the *.csproj file we were given actually exists.
            if (!File.Exists(projFilePath))
            {
                Console.WriteLine("The specified *.csproj file does not exist:");
                Console.WriteLine(projFilePath);
                return;
            }

            // Search for AndroidAsset elements within the project.
            var projXml = XDocument.Load(projFilePath);
            var projAssetElements = projXml.Descendants().Where(x => x.Name.LocalName == "AndroidAsset");
            var projAssetPaths = new List<String>();
            foreach (var projAsset in projAssetElements)
            {
                var path = String.Empty;
                var link = projAsset.Elements().Where(x => x.Name.LocalName == "Link").SingleOrDefault();
                if (link != null)
                {
                    path = link.Value.Replace('\\', Path.DirectorySeparatorChar);
                }
                else
                {
                    var include = projAsset.Attribute("Include");
                    if (include != null)
                    {
                        path = include.Value.Replace('\\', Path.DirectorySeparatorChar);
                    }
                }

                if (!String.IsNullOrEmpty(path))
                    projAssetPaths.Add(path);
            }

            // Build a hierarchy of files.
            var hierarchy = new Dictionary<String, HashSet<AndroidAssetInfo>>();
            foreach (var projAssetPath in projAssetPaths)
            {
                var file = true;
                var current = projAssetPath;
                while (!String.IsNullOrEmpty(current))
                {
                    var parent = Path.GetDirectoryName(current);
                    if (!hierarchy.TryGetValue(parent, out var children))
                    {
                        children = new HashSet<AndroidAssetInfo>();
                        hierarchy[parent] = children;
                    }

                    var name = Path.GetFileName(current);
                    children.Add(new AndroidAssetInfo(name, file));

                    file = false;
                    current = Path.GetDirectoryName(current);
                }
            }

            // Write the asset list.
            var outputFilePath = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(projFilePath)), "AndroidAssets.aalist");
            using (var stream = File.Create(outputFilePath))
            using (var writer = new StreamWriter(stream))
            {
                foreach (var node in hierarchy)
                {
                    foreach (var file in node.Value)
                    {
                        var type = file.IsFile ? "F" : "D";
                        writer.WriteLine($"{type}:{node.Key.Replace('\\', '/')}:{file.Name}");
                    }
                }
            }
            Console.WriteLine($"Android Asset file listing written to {outputFilePath}.");
        }
    }
}
