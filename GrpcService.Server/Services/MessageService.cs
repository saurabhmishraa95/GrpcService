using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService.Server.Services
{
    public class MessageService : Messanger.MessangerBase
    {
        public override async Task CreateChat(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            var readMessageTask = ReadMessage(requestStream, context);
            var writeMessageTask = WriteMessage(responseStream, context);

            await Task.WhenAll(readMessageTask, writeMessageTask);
        }

        private static async Task WriteMessage(IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            while (!context.CancellationToken.IsCancellationRequested)
            {
                var text = Console.ReadLine();
                await responseStream.WriteAsync(new Message { Text = text });
            }
        }

        private static async Task ReadMessage(IAsyncStreamReader<Message> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Recieved: {requestStream.Current.Text}");
            }
        }
    }
}
