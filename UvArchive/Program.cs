using System;
using System.IO;
using System.Linq;
using TwistedLogik.Ultraviolet.Content;

namespace UvArchive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                WriteHelp();
                return;
            }

            var output = args.First();
            var dirs   = args.Skip(1).ToList();

            if (dirs.Contains("-list"))
            {
                if (args.Length > 2)
                {
                    WriteHelp();
                    return;
                }
                ListContents(output);
                return;
            }

            var invalidInputs = dirs.Where(x => !Directory.Exists(x));
            if (invalidInputs.Any())
            {
                Console.WriteLine("The specified inputs do not exist or are not directories:");
                foreach (var invalidInput in invalidInputs)
                {
                    Console.WriteLine(" - {0}", invalidInput);
                }
                return;
            }

            if (!Path.HasExtension(output))
            {
                output = Path.ChangeExtension(output, "uvarc");
            }

            Console.Write("Generating {0}... ", output);

            var archive = ContentArchive.FromFileSystem(dirs);

            using (var stream = File.Open(output, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    archive.Save(writer);
                }
            }

            Console.WriteLine("done.");
        }

        private static void ListContents(String filename)
        {
            if (!Path.HasExtension(filename))
            {
                filename = Path.ChangeExtension(filename, "uvarc");
            }

            try
            {
                var archive = ContentArchive.FromArchiveFile(() => File.OpenRead(filename));
                foreach (var node in archive)
                {
                    ListContents(node, 0);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("File not found.");
            }
        }

        private static void ListContents(ContentArchiveNode node, Int32 indentation)
        {
            var indent = new String(' ', indentation);
            Console.WriteLine(indent + node.Name);

            foreach (ContentArchiveNode child in node.Children)
            {
                ListContents(child, indentation + 1);
            }
        }

        private static void WriteHelp()
        {
            Console.WriteLine("Generates Ultraviolet-compatible content archive files.");
            Console.WriteLine();
            Console.WriteLine("UVARCHIVE output dir1 [[dir2] [dir3]...]\n" +
                              "\n" +
                              "  output       The name of the generated archive file.\n" +
                              "  dir1 [[dir2] [dir3]...]\n" +
                              "               The directories to include as the root nodes of the\n" +
                              "               generated archive file.\n" +
                              "\n" +
                              "UVARCHIVE input -list\n" +
                              "\n" +
                              "  input        The name of the archive file to examine.\n" +
                              "  -list        Indicates that the contents of the specified\n" +
                              "               archive file should be written to the console.");
            Console.WriteLine();
        }
    }
}
