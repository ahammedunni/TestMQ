using System;
using Funq;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using ServiceStack.Testing;
using TestMqShared;

namespace TestMqCli
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var redisFactory = new PooledRedisClientManager("139.59.75.204:6379");
            var mqServer = new RedisMqServer(redisFactory, retryCount: 2);

            //Client - MQ Service Impl:
            //Listens for 'HelloResponse' returned by the 'Hello' Service
            mqServer.RegisterHandler<HelloResponse>(m => {
                Console.WriteLine("Received: " + m.GetBody().Result);
                // See comments below
                // m.Options = (int)MessageOption.None;
                return null;
            });

            //or to call an existing service with:
            //mqServer.RegisterHandler<HelloResponse>(m =>   
            //    this.ServiceController.ExecuteMessage(m));

            mqServer.Start(); //Starts listening for messages

            var mqClient = mqServer.CreateMessageQueueClient();
            mqClient.Publish(new Hello { Name = "Client 1" });

            Console.WriteLine("Client running.  Press any key to terminate...");
            Console.ReadLine(); //Prevent self-hosted Console App from exiting
        }
    }
}