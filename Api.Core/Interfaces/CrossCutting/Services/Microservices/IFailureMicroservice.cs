using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Api.Core.Services.Microservices
{
    public interface IFailureMicroservice
    {
        public Task SendMessageAsync(Exception exceptionObject);
    }
}
