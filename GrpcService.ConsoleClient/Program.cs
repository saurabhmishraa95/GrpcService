using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcService.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);
            //var reply = await client.SayHelloAsync(new HelloRequest { Name = "Saurabh" });
            //Console.WriteLine(reply.Message);
            //for (int i = 0; i < 10; i++)
            //{
            //    Thread.Sleep(2000);
            //    secReply = await secClient.GetCustomerAsync(new CustomerRequest { Userid = i });
            //    Console.WriteLine($"Id: {secReply.Id} Name: {secReply.Name} Email: {secReply.Email}");
            //}

            // streaming using grpc
            //var customerClient = new CustomerInfo.CustomerInfoClient(channel);
            //using (var reply = customerClient.GetCustomerStream(new EmptyRequest()))
            //{
            //    CustomerResponse customerResponse;
            //    while(await reply.ResponseStream.MoveNext(new CancellationToken()))
            //    {
            //        customerResponse = reply.ResponseStream.Current;
            //        Console.WriteLine($"Id: {customerResponse.Id} Name: {customerResponse.Name} Email: {customerResponse.Email}");
            //    }
            //}

            var chatClient = new Messanger.MessangerClient(channel);
            using (var chat = chatClient.CreateChat())
            {
                var readMessageTask = ReadMessage(chat);
                var writeMessageTask = WriteMessage(chat);

                await Task.WhenAll(readMessageTask, writeMessageTask);

            }

            //Console.ReadKey();
        }

        private static async Task ReadMessage(AsyncDuplexStreamingCall<Message, Message> chat)
        {
            var cancellationToken = new CancellationToken();
            while (await chat.ResponseStream.MoveNext(cancellationToken))
            {
                Console.WriteLine($"Recieved: {chat.ResponseStream.Current.Text}");
            }
        }

        private static async Task WriteMessage(AsyncDuplexStreamingCall<Message, Message> chat)
        {
            while (true)
            {
                string text = Console.ReadLine();
                await chat.RequestStream.WriteAsync(new Message { Text = text });
            }
        }
    }
}
