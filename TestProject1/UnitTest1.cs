using PopToRelatedFile;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
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
                    <script src='~/script1.js'></script>
                    <script src=""~/script2.js""></script>
                    <script src=""https://example.com/script.js""></script>
                    <script>console.log('Inline script');</script>
                </head>
                <body></body>
            </html>");

            var solutionServiceMock = new Mock<ISolutionService>();
            solutionServiceMock.Setup(s => s.GetAllFilesAsync()).Returns(new List<SolutionItem>());

            var detector = new CshtmlLinkedJsRelatedFileDetector(documentServiceMock.Object, solutionServiceMock.Object);

            var textDocumentMock = new Mock<ITextDocument>();

            var relatedFiles = await detector.CorrespondingFiles(textDocumentMock.Object);

            Assert.AreEqual(2, relatedFiles.Count);
            Assert.AreEqual("https://example.com/script1.js", relatedFiles[0]);
            Assert.AreEqual("https://example.com/script2.js", relatedFiles[1]);
        }
    }
}