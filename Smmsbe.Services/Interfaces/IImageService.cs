using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Interfaces
{
    public interface IImageService
    {
        Task<bool> UploadImageAsync(string path, Stream fsStream);
    }
}
