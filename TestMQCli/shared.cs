using System;

namespace TestMqShared
{
    public class Hello
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



    public class EmailRequest
    {
        public string FromAdressTitle { get; set; }
        public string ToAddress { get; set; }
        public string ToAdressTitle { get; set; }
        public string Subject { get; set; }
        public string BodyContent { get; set; }
    }
    public class EmailResponse
    {
        public string Result { get; set; }
    }
    public class EmailError
    {
        public string Result { get; set; }
    }
}