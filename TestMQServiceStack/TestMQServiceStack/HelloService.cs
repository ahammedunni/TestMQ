using System;
using ServiceStack;
using TestMqShared;

namespace TestMQServiceStack
{
    class HelloService : Service
    {
        public object Any(Hello req)
        {
            try
            {
                HelloResponse res = new HelloResponse();
                res.Result = req.Name + "  Success";
                if (req.Name.Contains("a= 5"))
                    throw new Exception();
                Console.WriteLine(res.Result);
                return res;
            }
            catch(Exception E)
            {
                HelloError res = new HelloError();
                res.Result = E.ToJson();
                Console.WriteLine(res.Result);
                return res;
            }

        }

    }
}