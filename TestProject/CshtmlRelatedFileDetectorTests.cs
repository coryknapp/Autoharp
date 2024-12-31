using Microsoft.VisualStudio.Text;
using Moq;
using Autoharp.Services;
using Autoharp.Models;
using File = Autoharp.Models.File;

namespace TestProject
{
    [TestClass]
    public class CshtmlRelatedFileDetectorTests
    {
        [TestMethod]
        public async Task Cshtml_to_cs_RelatedFileDetectorTestsAsync()
        {

            var documentService = DocumentServiceHelper.GetDocumentServiceMock();

            var detector = new CshtmlToCsFileDetector(documentService.Object);

            var originFile = new File("C:\\code\\test.cshtml");

            var relatedFiles = (await detector.CorrespondingFilesAsync(originFile)).ToList();

            Assert.AreEqual(1, relatedFiles.Count);
            Assert.AreEqual("C:\\code\\test.cshtml.cs", relatedFiles.First().FullPath);
        }
    }
}