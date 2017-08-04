using MailKit.Net.Smtp;
using MimeKit;
using ServiceStack;
using System;
using TestMqShared;

namespace TestMQServiceStack
{
    public class EmailService : Service
    {
         public object Any(EmailReq req) {

            EmailRes resp = new EmailRes();

            string FromAddress = "binivarghese@expressbase.com";
            string FromAdressTitle = "ExpressBase";
            //To Address  
            string ToAddress = "ahammedunni@expressbase.com";
            string ToAdressTitle = "Ahammed Unni V P";
            string Subject = "Message Queue Test";
            string BodyContent = "The Message here is not of significance. It's just a test message from message queue";
           
            string SmtpServer = "smtp.zoho.com";
            //Smtp Port Number  
            int SmtpPortNumber = 465;

            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
            mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
            mimeMessage.Subject = Subject;
            mimeMessage.Body = new TextPart("plain")
            {
                Text = BodyContent
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(SmtpServer, SmtpPortNumber, true);
                    // Note: only needed if the SMTP server requires authentication  
                    // Error 5.5.1 Authentication   
                    client.Authenticate("binivarghese@expressbase.com", "1.Pappaamma");
                    client.Send(mimeMessage);
                    Console.WriteLine("The mail has been sent successfully !!");

                    client.Disconnect(true);

                    resp.Name = "Success By Unni";

                }
                catch (Exception e)
                {
                    resp.ResponseStatus = new ResponseStatus { Message = e.Message };
                    throw new Exception("Unni Excpetion");
                }
            }

            return resp;
        }
    }
}