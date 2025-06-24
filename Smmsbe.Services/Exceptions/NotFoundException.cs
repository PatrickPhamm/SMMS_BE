using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string msgId, string message) : base(msgId, message)
        {
        }
    }
}
