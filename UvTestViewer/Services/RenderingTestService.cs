using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UvTestViewer.Models;

namespace UvTestViewer.Services
{
    /// <summary>
    /// Represents a service which retrieves and processes rendering test data.
    /// </summary>
    public class RenderingTestService
    {
        /// <summary>
        /// Gets an overview of the most recent rendering test run.
        /// </summary>
        /// <param name="vendor">The vendor for which to retrieve a rendering test run.</param>
        /// <returns>A <see cref="RenderingTestOverview"/> instance which represents the msot recent test run.</returns>
        public RenderingTestOverview GetMostRecentRenderingTestOverview(GpuVendor vendor)
        {
            var id        = 0L;
            var directory = GetMostRecentTestResultsDirectory(vendor, out id);
            if (directory == null)
                return null;

            var images       = directory.GetFiles("*.png");
            var imagesByTest = from file in images
                               let filename = Path.GetFileName(file.FullName)
                               let testname = GetTestNameFromImageFileName(file.FullName)
                               where
                                !String.IsNullOrEmpty(testname)
                               group filename by testname into g
                               select g;

            var outputdir = Path.Combine("TestResults", vendor.ToString(), id.ToString());

            var failedTests = GetFailedTests(directory);

            var tests = new List<RenderingTest>();
            foreach (var imageGroup in imagesByTest)
            {
                var testExpected = imageGroup.Where(x => x.EndsWith("_Expected.png")).SingleOrDefault();
                var testActual   = imageGroup.Where(x => x.EndsWith("_Actual.png")).SingleOrDefault();
                var testDiff     = imageGroup.Where(x => x.EndsWith("_Diff.png")).SingleOrDefault();

                var test = new RenderingTest(imageGroup.Key,
                    GetRelativeUrlOfImage(outputdir, testExpected),
                    GetRelativeUrlOfImage(outputdir, testActual),
                    GetRelativeUrlOfImage(outputdir, testDiff));

                test.Failed = failedTests.Contains(imageGroup.Key);

                tests.Add(test);
            }

            return new RenderingTestOverview()
            {
                TestRunID = id,
                Tests = tests,
                TimeProcessed = directory.CreationTime,
                Vendor = vendor
            };
        }

        /// <summary>
        /// Gets the test run identifier associated with the specified directory.
        /// </summary>
        /// <param name="name">The name of the directory to evaluate.</param>
        /// <returns>The test run identifier associated with the specified directory, or <c>null</c> if the
        /// specified directory is not associated with a test run.</returns>
        private static Int64? GetDirectoryID(String name)
        {
            Int64 id;

            if (!Int64.TryParse(name, out id))
                return null;

            return id;
        }

        /// <summary>
        /// Gets a <see cref="DirectoryInfo"/> which represents the most recent rendering test run.
        /// </summary>
        /// <param name="vendor">The vendor for which to retrieve a rendering test run.</param>
        /// <param name="id">The identifier of the test run associated with the retrieved directory.</param>
        /// <returns>A <see cref="DirectoryInfo"/> which represents the most recent rendering test run.</returns>
        private static DirectoryInfo GetMostRecentTestResultsDirectory(GpuVendor vendor, out Int64 id)
        {
            var root = ConfigurationManager.AppSettings["TestResultRootDirectory"];

            switch (vendor)
            {
                case GpuVendor.Intel: 
                    root = Path.Combine(root, "Intel"); 
                    break;
                
                case GpuVendor.Nvidia: 
                    root = Path.Combine(root, "Nvidia"); 
                    break;
                
                case GpuVendor.Amd: 
                    root = Path.Combine(root, "Amd"); 
                    break;

                default: 
                    throw new ArgumentException("Unrecognized GPU hardware vendor.");
            }

            if (!Directory.Exists(root))
            {
                id = 0;
                return null;
            }

            var rootSubdirs     = Directory.GetDirectories(root);
            var rootSubdirsByID = from subdir in rootSubdirs
                                  let dirInfo = new DirectoryInfo(subdir)
                                  let dirID = GetDirectoryID(dirInfo.Name)
                                  where dirID.HasValue
                                  orderby dirInfo.CreationTime descending
                                  select new { ID = dirID, DirectoryInfo = dirInfo };

            var directory = rootSubdirsByID.FirstOrDefault();
            if (directory == null)
            {
                id = 0;
                return null;
            }
            id = directory.ID.Value;
            return directory.DirectoryInfo;
        }

        /// <summary>
        /// Gets the relative URL used to display the specified unit test image.
        /// </summary>
        /// <param name="rootdir">The root directory of unit test images for the current run.</param>
        /// <param name="image">The path to the image for which to retrieve a URL.</param>
        /// <returns>The relative URL used to display the specified unit test image.</returns>
        private static String GetRelativeUrlOfImage(String rootdir, String image)
        {
            return String.IsNullOrEmpty(image) ? null : Path.Combine(rootdir, image);
        }

        /// <summary>
        /// Gets the name of the test associated with the specified output image.
        /// </summary>
        /// <param name="filename">The filename of the output image to evaluate.</param>
        /// <returns>The name of the test associated with the specified output image.</returns>
        private static String GetTestNameFromImageFileName(String filename)
        {
            var filenameNoExt = Path.GetFileNameWithoutExtension(filename);

            if (filenameNoExt.EndsWith("_Actual"))
                return filenameNoExt.Substring(0, filenameNoExt.Length - "_Actual".Length);

            if (filenameNoExt.EndsWith("_Expected"))
                return filenameNoExt.Substring(0, filenameNoExt.Length - "_Expected".Length);

            if (filenameNoExt.EndsWith("_Diff"))
                return filenameNoExt.Substring(0, filenameNoExt.Length - "_Diff".Length);

            return null;
        }

        /// <summary>
        /// Gets a set containing the name of any failing tests.
        /// </summary>
        /// <param name="dir">The directory that contains the test run.</param>
        /// <returns>A <see cref="HashSet{String}"/> containing the names of any failing tests.</returns>
        private static HashSet<String> GetFailedTests(DirectoryInfo dir)
        {
            var cacheFilename = Path.Combine(dir.FullName, "ResultCache.txt");
            try
            {
                var cacheText = File.ReadAllLines(cacheFilename);
                return new HashSet<String>(cacheText);
            }
            catch (IOException) { }

            try
            {
                var resultFilename = Path.Combine(dir.FullName, "Result.trx");
                var resultXml      = XDocument.Load(resultFilename);
                var resultNodes    = resultXml.Descendants("UnitTestResult");

                var names = from r in resultNodes
                            let testName = (String)r.Attribute("testName")
                            let outcome  = (String)r.Attribute("outcome")
                            where 
                                outcome == "Failed"
                            select testName;

                File.WriteAllLines(cacheFilename, names);

                return new HashSet<String>(names);
            }
            catch (IOException)
            {
                return new HashSet<String>();
            }
        }
    }
}
