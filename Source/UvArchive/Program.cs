using System;
using System.IO;
using System.Linq;
using Ultraviolet.Content;
using Ultraviolet.Tooling;

namespace UvArchive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ArchiveGenerationParameters parameters;
            try { parameters = new ArchiveGenerationParameters(args); }
            catch (InvalidCommandLineException e)
            {
                if (String.IsNullOrEmpty(e.Error))
                {
                    Console.WriteLine("Generates Ultraviolet-compatible content archive files.");
                    Console.WriteLine();
                    Console.WriteLine("UVARCHIVE -pack output dir1 [[dir2] [dir3]...]\n" +
                              "  Packs the specified list of directories into an archive file.\n" +
                              "\n" +
                              "  output       The name of the generated archive file.\n" +
                              "  dir1 [[dir2] [dir3]...]\n" +
                              "               The directories to include as the root nodes of the\n" +
                              "               generated archive file.\n" +
                              "\n" +
                              "UVARCHIVE -list input\n" +
                              "  Writes the contents of the specified archive file to the console.\n" +
                              "\n" +
                              "  input        The name of the archive file to examine.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine(e.Error);
                }
                return;
            }

            switch (parameters.Command)
            {
                case ArchiveGenerationCommand.Pack:
                    PackArchive(parameters);
                    break;

                case ArchiveGenerationCommand.List:
                    ListArchive(parameters);
                    break;
            }
        }

        private static void PackArchive(ArchiveGenerationParameters parameters)
        {
            var dirs = parameters.PackDirectories;
            var output = parameters.PackOutput;

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

        private static void ListArchive(ArchiveGenerationParameters parameters)
        {
            var filename = parameters.ListInput;

            if (!Path.HasExtension(filename))
            {
                filename = Path.ChangeExtension(filename, "uvarc");
            }

            try
            {
                var archive = ContentArchive.FromArchiveFile(() => File.OpenRead(filename));
                foreach (var node in archive)
                {
                    ListArchive(node, 0);
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

        private static void ListArchive(ContentArchiveNode node, Int32 indentation)
        {
            var indent = new String(' ', indentation);
            Console.WriteLine(indent + node.Name);

            foreach (ContentArchiveNode child in node.Children)
            {
                ListArchive(child, indentation + 1);
            }
        }
    }
}