using Api.Core.Services.Microservices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.CrossCutting.Services.Microservices
{
    public class FailureMicroservice : IFailureMicroservice
    {
        private readonly IConfiguration _configuration;
        static IQueueClient _queueClient;
        public FailureMicroservice(IConfiguration configuration)
        {
            _configuration = configuration;
            setupQueue();
        }

        private void setupQueue()
        {
            var queueName = _configuration.GetSection("Settings:MicroServices:FailureMicroservice:QueueName").Value;
            var ConnectionString = _configuration.GetSection("Settings:MicroServices:FailureMicroservice:ConnectionString").Value;
            _queueClient = new QueueClient(ConnectionString, queueName);
        }

        public async Task SendMessageAsync (Exception exceptionObject)
        {
            try
            {
                    // Create a new message to send to the queue
                    string messageBody = JsonConvert.SerializeObject(exceptionObject);
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Send the message to the queue
                    await _queueClient.SendAsync(message);
                
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
