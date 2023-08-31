using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aws.Tools.Message.SQS
{
    public class SQSMessage : Amazon.SQS.Model.Message
    {
        public override string ToString()
        {
            return $"[MessageId]: {MessageId}, [MessageBody]: {Body}";
        }
    }
}
