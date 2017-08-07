using System;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using TestMqShared;

namespace TestMqCli
{
    class MainClass
    {
        public static void Main(string[] args)
        {            
            var redisFactory = new PooledRedisClientManager("139.59.75.204:6379");
            var mqServer = new RedisMqServer(redisFactory, retryCount: 2);
            var mqClient = mqServer.CreateMessageQueueClient(); //Creates a Redis Message Queue Client 

            //Client - MQ Service Impl:
            //Listens for 'HelloResponse' returned by the 'Hello' Service

            mqServer.RegisterHandler<HelloResponse>(m =>
            {
                //Console.WriteLine(m.Body.ToSafeJson());
                //See comments below
                 m.Options = (int)MessageOption.None;
                return null;
            });


            mqServer.RegisterHandler<EmailResponse>(m =>
            {
                //Console.WriteLine(m.Body.ToSafeJson());
                //See comments below
                m.Options = (int)MessageOption.None;
                return null;
            });

            //Listens for 'HelloError' returned by the 'Hello' Service
            mqServer.RegisterHandler<HelloError>(m => {
                //Console.WriteLine(m.Body.ToSafeJson());
                // See comments below
                m.Options = (int)MessageOption.None;

                var ErrorEmail = new Message<EmailRequest>(new EmailRequest {
                    FromAdressTitle = "Message Queue",
                    ToAddress = "ahammedunni@expressbase.com",
                    ToAdressTitle ="Ahammed Unni",
                    Subject = "Hello Error",
                    BodyContent = m.Body.ToString()
                });
                return ErrorEmail;
            });

            mqServer.RegisterHandler<EmailError>(m => {
                Console.WriteLine(m.Body.ToSafeJson());
                // See comments below
                m.Options = (int)MessageOption.None;
                return null;
            });

            //or to call an existing service with:
            //mqServer.RegisterHandler<HelloResponse>(m =>   
            //    this.ServiceController.ExecuteMessage(m));

            mqServer.Start(); //Starts listening for messages

            for (int a =1; a<10; a++) {

                var HelloMessage = new Message<Hello>(new Hello { Name = "Unni Priority  a= " + a });

                mqClient.Publish((QueueNames<Hello>.Priority), HelloMessage); // QueueNames<Hello>.Priority=mq:Hello.priorityq

                for (int b = 1; b < 10; b++) { 
                    HelloMessage.Body = new Hello { Name ="Unni  a= " + a + "   b= " + b};
                    mqClient.Publish((QueueNames<Hello>.In), HelloMessage);
                }
            }

            var EmailMessage = new Message<EmailRequest>(new EmailRequest
            {
                FromAdressTitle = "Message Queue",
                ToAddress = "ahammedunni@expressbase.com",
                ToAdressTitle = "Ahammed Unni",
                Subject = "Message Queue Email Success",
                BodyContent = "This is a success Notification processed through the message queue."
            });
            mqClient.Publish((QueueNames<EmailRequest>.In), EmailMessage);
            //mq:EmailRequest.In


            Console.WriteLine("Client running.  Press any key to terminate...");
            //Prevent self-hosted Console App from exiting
            Console.ReadLine();
            //IMessage<HelloResponse> EmailResponse = mqClient.GetAsync<HelloResponse>(QueueNames<Hello>.Dlq);
            //Console.WriteLine(EmailResponse.ToSafeJson());



        }
    }
}