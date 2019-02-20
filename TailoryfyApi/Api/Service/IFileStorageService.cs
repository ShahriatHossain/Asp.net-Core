using System.IO;
using System.Threading.Tasks;

namespace Api.Service
{
    public interface IFileStorageService
    {
        Task Upload(Stream stream, string blobName);
    }
}
