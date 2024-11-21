using Microsoft.VisualStudio.Text;
using Moq;
using PopToRelatedFile.Services;

namespace TestProject
{
    [TestClass]
    public class CshtmlRelatedFileDetectorTests
    {
        [TestMethod]
        public async Task Cshtml_to_cs_RelatedFileDetectoTestsAsync()
        {
            var documentServiceMock = new Mock<IDocumentService>();
            documentServiceMock.Setup(s => s.IsType(It.IsAny<ITextDocument>(), It.IsAny<string>())).Returns(true);
            documentServiceMock.Setup(s => s.FileExists(It.IsAny<string>())).Returns(true);
            documentServiceMock.Setup(s => s.FilePath(It.IsAny<ITextDocument>())).Returns(Task.FromResult("C:\\code\\test.cshtml"));

            var detector = new CshtmlRelatedFileDetector(documentServiceMock.Object);

            var textDocumentMock = new Mock<ITextDocument>();

            var relatedFiles = await detector.CorrespondingFiles(textDocumentMock.Object);

            Assert.AreEqual(1, relatedFiles.Count);
            Assert.AreEqual("C:\\code\\test.cshtml.cs", relatedFiles.First());
        }
    }
}