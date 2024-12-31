using Microsoft.VisualStudio.Text;
using Moq;
using Autoharp.Services;
using Autoharp.Models;
using File = Autoharp.Models.File;

namespace TestProject
{
    [TestClass]
    public class CsRelatedFileDetectorTests
    {
        [TestMethod]
        public async Task Cs_to_cshtml_RelatedFileDetectorTestsAsync()
        {
            var documentServiceMock = DocumentServiceHelper.GetDocumentServiceMock();

            var originFile = new File("C:\\code\\test.cshtml.cs");

            var detector = new CsRelatedFileDetector(documentServiceMock.Object);

            var textDocumentMock = new Mock<ITextDocument>();

            var relatedFiles = (await detector.CorrespondingFilesAsync(originFile)).ToList();

            Assert.AreEqual(1, relatedFiles.Count);
            Assert.AreEqual("C:\\code\\test.cshtml", relatedFiles.First().FullPath);
        }
    }
}