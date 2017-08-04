using System;

namespace TestMqShared
{
    public class Hello
    {
        public string Name { get; set; }
    }
    public class EmailReq
    {
        public string Name { get; set; }
    }
    public class EmailRes
    {
        public string Name { get; set; }
    }
    public class HelloResponse
    {
        public string Result { get; set; }
    }
    public class HelloError
    {
        public string Result { get; set; }
    }
}