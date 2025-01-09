using Microsoft.VisualStudio.Text;
using Moq;
using Autoharp.Services;
using Autoharp.Models;
using File = Autoharp.Models.File;
using static System.Net.Mime.MediaTypeNames;

namespace TestProject
{
    [TestClass]
    public class CsInterfaceFileDetectorTests
    {
        [TestMethod]
        public async Task CsInterfaceFileDetectorTestAsync()
        {
            var documentServiceMock = DocumentServiceHelper.GetDocumentServiceMock(
                null,
                @"
                    namespace ns
                    {
                        public interface I {}
                        public class A : I {}
                    }"
                );

            var originFile = new File("C:\\code\\test.cshtml.cs");

            var detector = new CsInterfaceFileDetector(documentServiceMock.Object);

            var textDocumentMock = new Mock<ITextDocument>();

            try
            {
                var test = await detector.CorrespondingFilesAsync(originFile);
                //Assert.AreEqual(1, test.ToList().Count);
                //Assert.AreEqual("C:\\code\\test.cshtml", test.ToList().First().FullPath);
            }
            catch (Exception ex)
            {
            }

        }
    }
}