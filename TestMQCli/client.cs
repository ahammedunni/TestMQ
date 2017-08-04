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

            mqServer.RegisterHandler<HelloResponse>(m =>
            {
                //Console.WriteLine(m.Body.ToSafeJson());
                //See comments below
                 m.Options = (int)MessageOption.None;
                return null;
            });

            //Listens for 'HelloError' returned by the 'Hello' Service
            mqServer.RegisterHandler<HelloError>(m => {
                Console.WriteLine(m.Body.ToSafeJson());
                // See comments below
                m.Options = (int)MessageOption.None;
                return null;
            });

            //or to call an existing service with:
            //mqServer.RegisterHandler<HelloResponse>(m =>   
            //    this.ServiceController.ExecuteMessage(m));

            mqServer.Start(); //Starts listening for messages
            
            
            

            var mqClient = mqServer.CreateMessageQueueClient(); //Creates a Redis Message Queue Client 


            for (int a =1; a<10; a++) {

                IMessage Msg = new Message<Hello>(new Hello { Name = "Unni Priority  a= " + a });

                mqClient.Publish((QueueNames<Hello>.Priority), Msg); // QueueNames<Hello>.Priority=mq:Hello.priorityq

                for (int b = 0; b < 10; b++) { 
                    Msg.Body = new Hello { Name ="Unni  a= " + a + "   b= " + b};
                    mqClient.Publish((QueueNames<Hello>.In), Msg);
                }
            }                  
           
            Console.WriteLine("Client running.  Press any key to terminate...");
            //Prevent self-hosted Console App from exiting
            Console.ReadLine();
            //IMessage<HelloResponse> EmailResponse = mqClient.GetAsync<HelloResponse>(QueueNames<Hello>.Dlq);
            //Console.WriteLine(EmailResponse.ToSafeJson());



        }
    }
}