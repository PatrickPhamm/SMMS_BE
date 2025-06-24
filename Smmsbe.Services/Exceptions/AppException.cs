using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smmsbe.Services.Exceptions
{
    public abstract class AppException : Exception
    {
        public string MessageId { get; set; }

        protected AppException(string msgId, string message) : base(message)
        {
            MessageId = msgId;
        }
    }
}
