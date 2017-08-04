using System;
using Funq;
using ServiceStack;
using ServiceStack.Messaging.Redis;
using ServiceStack.Redis;
using TestMqShared;

namespace TestMq
{
    //Server
    //public class HelloService : Service
    //{
    //    public object Any(EmailReq req)
    //    {
    //        EmailService mailreq = new EmailService();
    //        return new EmailRes { Name = "Hello, " + req.Name };
    //    }
    //}

    public class ServerAppHost : AppHostHttpListenerBase
    {
        public ServerAppHost() : base("Test Server", typeof(ServerAppHost).Assembly){}


        public override void Configure(Container container)
        {
            //base.Routes
            //    .Add<Hello>("/hello")
            //    .Add<Hello>("/hello/{Name}");

            var redisFactory = new PooledRedisClientManager("139.59.75.204:6379");
            container.Register<IRedisClientsManager>(redisFactory);
            var mqHost = new RedisMqServer(redisFactory, retryCount: 2);

            //Server - MQ Service Impl:

            //Listens for 'Hello' messages sent with: mqClient.Publish(new Hello { ... })

            mqHost.RegisterHandler<Hello>(base.ExecuteMessage);
            mqHost.RegisterHandler<EmailReq>(base.ExecuteMessage);
            mqHost.Start(); //Starts listening for messages
        }
    }

    class MainClass
    {
        
        public static void Main(string[] args)
        {
            System.Threading.Thread.Sleep(5000);

            var serverAppHost = new ServerAppHost();
            serverAppHost.Init();
            serverAppHost.Start("http://localhost:1400/");
            Console.WriteLine("Server running.  Press enter to terminate...");
            //Prevent server from exiting (when running in ConsoleApp)
            Console.ReadLine();
        }
    }
}