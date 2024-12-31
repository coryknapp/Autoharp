using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Text;
using Moq;
using PopToRelatedFile.Services;
using PopToRelatedFile.Models;
using File = PopToRelatedFile.Models.File;

namespace TestProject
{
    [TestClass]
    public class CshtmlLinkedJsRelatedFileDetectorTests
    {
        [TestMethod]
        public async Task CshtmlLinkedJsRelatedFileDetectorTestsAsync()
        {
            var documentServiceMock = DocumentServiceHelper.GetDocumentServiceMock(
                new List<string>()
                {
                    "~/www/scripts/script1.js",
                    "~/www/scripts/script2.js",
                },
                @"<html>
                <head>
                    <script src='~/script1.js'></script>
                    <script src=""~/script2.js""></script>
                    <script src='~/not-found.js'></script>
                    <script src=""https://example.com/script.js""></script>
                    <script>console.log('In-line script');</script>
                </head>
                <body></body>
            </html>");

            var detector = new CshtmlLinkedJsRelatedFileDetector(documentServiceMock.Object);

            var originFile = new File("");

            var relatedFiles = (await detector.CorrespondingFilesAsync(originFile)).ToList();

            Assert.AreEqual(2, relatedFiles.Count);
            Assert.AreEqual("~/www/scripts/script1.js", relatedFiles[0].FullPath);
            Assert.AreEqual("~/www/scripts/script2.js", relatedFiles[1].FullPath);
        }
    }
}