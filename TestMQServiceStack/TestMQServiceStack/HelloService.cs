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
                if (req.Name.Contains("a= 2   b= 2"))
                    throw new Exception("Manually Generated Error to Test Error Handler created at a= 5   b= 5");
                Console.WriteLine(res.Result);
                return res;
            }
            catch(Exception E)
            {
                HelloError Error = new HelloError();
                Error.Result = E.ToJson();
                Console.WriteLine(Error.Result);
                return Error;
            }

        }

    }
}