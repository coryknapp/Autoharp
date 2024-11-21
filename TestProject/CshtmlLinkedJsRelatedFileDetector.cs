using Microsoft.VisualStudio.Text;
using Moq;
using PopToRelatedFile.Services;

namespace TestProject
{
    [TestClass]
    public class CshtmlLinkedJsRelatedFileDetectorTests
    {
        [TestMethod]
        public async Task CshtmlLinkedJsRelatedFileDetectorTestsAsync()
        {
            var documentServiceMock = new Mock<IDocumentService>();
            documentServiceMock.Setup(s => s.IsType(It.IsAny<ITextDocument>(), It.IsAny<string>())).Returns(true);
            documentServiceMock.Setup(s => s.FileExists(It.IsAny<string>())).Returns(true);
            documentServiceMock.Setup(s => s.FilePath(It.IsAny<ITextDocument>())).Returns(Task.FromResult("C:\\code\\test.cshtml"));
            documentServiceMock.Setup(s => s.GetDocumentText(It.IsAny<ITextDocument>())).Returns(
                @"
            <html>
                <head>
                    <script src='https://example.com/script1.js'></script>
                    <script src=""https://example.com/script2.js""></script>
                    <script>console.log('Inline script');</script>
                </head>
                <body></body>
            </html>");


            var detector = new CshtmlLinkedJsRelatedFileDetector(documentServiceMock.Object);

            var textDocumentMock = new Mock<ITextDocument>();

            var relatedFiles = await detector.CorrespondingFiles(textDocumentMock.Object);

            Assert.AreEqual(2, relatedFiles.Count);
            Assert.AreEqual("https://example.com/script1.js", relatedFiles[0]);
            Assert.AreEqual("https://example.com/script2.js", relatedFiles[1]);
        }
    }
}