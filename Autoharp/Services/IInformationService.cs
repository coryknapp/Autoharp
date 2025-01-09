using Autoharp.Models;
using System.Collections.Generic;

namespace Autoharp.Services
{
    public interface IInformationService
    {
        Task InformResetOriginAsync(string originFilePath, IEnumerable<File> relatedFileList);
        Task LogErrorAsync(string message);
    }
}