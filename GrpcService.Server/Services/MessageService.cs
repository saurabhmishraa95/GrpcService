using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Server.Services
{
    public class MessageService : Messanger.MessangerBase
    {
        public override async Task SendMessage(Message request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            while (true)
            {
                string text = Console.ReadLine();
                var message = new Message { Text = text };
                await responseStream.WriteAsync(message);
            }
        }
    }
}
