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

            using (var archive = ContentArchive.FromFileSystem(dirs))
            {
                using (var stream = File.Open(output, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        archive.Save(writer);
                    }
                }
            }

            Console.WriteLine("done.");
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
                                          "               generated archive file.");
            Console.WriteLine();
        }
    }
}
