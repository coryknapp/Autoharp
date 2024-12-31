using Moq;
using Autoharp.Services;
using Autoharp.Models;
using File = PopToRelatedFile.Models.File;

namespace TestProject
{
    internal class DocumentServiceHelper
    {
        public static Mock<IDocumentService> GetDocumentServiceMock(IEnumerable<string>? existingFilePaths = null, string? documentText = null)
        {
            var documentServiceMock = new Mock<IDocumentService>();

            if (existingFilePaths != null)
            {
                documentServiceMock.Setup(s => s.GetAllFilesAsync(It.IsAny<Func<File, bool>>()))
                    .Returns(Task.FromResult(existingFilePaths.Select(p => new File(p))));
            } else
            {
                documentServiceMock.Setup(s => s.GetAllFilesAsync(It.IsAny<Func<File, bool>>()))
                    .Returns(Task.FromResult(Enumerable.Empty<File>()));
            }

            documentServiceMock.Setup(s => s.GetDocumentTextAsync(It.IsAny<File>()))
                .Returns(Task.FromResult(documentText));

            documentServiceMock.Setup(s => s.IsTypeAsync(It.IsAny<File>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            documentServiceMock.Setup(s => s.FileExists(It.IsAny<File>()))
                .Returns(true);

            return documentServiceMock;
        }
    }
}
