using System;
using System.Collections.Generic;
using System.Linq;
using Ultraviolet.Tooling;

namespace UvArchive
{
    /// <summary>
    /// Represents the user-specified parameters which are used to determine how the archive is generated.
    /// </summary>
    public class ArchiveGenerationParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveGenerationParameters"/> class.
        /// </summary>
        /// <param name="args">The application's command line arguments.</param>
        public ArchiveGenerationParameters(String[] args)
        {
            if (args == null || !args.Any())
                throw new InvalidCommandLineException();

            var parser = new CommandLineParser(args);
            if (!parser.IsParameter(args.First()))
                throw new InvalidCommandLineException();

            switch (args.First().ToLowerInvariant())
            {
                case "-pack":
                    Command = ArchiveGenerationCommand.Pack;
                    ProcessPackParameters(args, parser);
                    break;

                case "-list":
                    Command = ArchiveGenerationCommand.List;
                    ProcessListParameters(args, parser);
                    break;

                default:
                    throw new InvalidCommandLineException();
            }
        }

        /// <summary>
        /// Gets the archive generation command.
        /// </summary>
        public ArchiveGenerationCommand Command { get; private set; }

        /// <summary>
        /// Gets the name of the output file for the pack command.
        /// </summary>
        public String PackOutput { get; private set; }

        /// <summary>
        /// Gets the collection of input directories for the pack command.
        /// </summary>
        public IEnumerable<String> PackDirectories { get; private set; }

        /// <summary>
        /// Gets the name of the archive to examine with the list command.
        /// </summary>
        public String ListInput { get; private set; }

        /// <summary>
        /// Processes user-specified parameters for the pack command.
        /// </summary>
        /// <param name="args">The application's command line arguments.</param>
        /// <param name="parser">The command line argument parser.</param>
        private void ProcessPackParameters(String[] args, CommandLineParser parser)
        {
            PackOutput = args.Skip(1).Take(1).Single();
            PackDirectories = args.Skip(2).ToList();
        }

        /// <summary>
        /// Processes user-specified parameters for the list command.
        /// </summary>
        /// <param name="args">The application's command line arguments.</param>
        /// <param name="parser">The command line argument parser.</param>
        private void ProcessListParameters(String[] args, CommandLineParser parser)
        {
            ListInput = args.Skip(1).Take(1).Single();
        }
    }
}