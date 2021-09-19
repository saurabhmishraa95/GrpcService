using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcService.Server
{
    public class CustomerService : CustomerInfo.CustomerInfoBase
    {
        public override Task<CustomerResponse> GetCustomer(CustomerRequest request, ServerCallContext context)
        {
            Console.WriteLine(request.Userid);
            return Task.FromResult(new CustomerResponse
            {
                Id = request.Userid,
                Email = request.Userid + "@gmail.com",
                HasPurchaseHistory = true,
                Name = "Ramesh"
            });
        }

        public override async Task GetCustomerStream(EmptyRequest request, 
            IServerStreamWriter<CustomerResponse> responseStream, ServerCallContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                await responseStream.WriteAsync(new CustomerResponse { Id = i, Email = i + "@s.com" });
                Thread.Sleep(1000);
            }
        }
    }
}
