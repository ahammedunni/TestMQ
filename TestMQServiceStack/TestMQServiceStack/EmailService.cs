using MailKit.Net.Smtp;
using MimeKit;
using ServiceStack;
using System;
using TestMqShared;

namespace TestMQServiceStack
{
    public class EmailService : Service
    {
         public object Any(EmailRequest req) {

            EmailResponse resp = new EmailResponse();

            string FromAddress = "binivarghese@expressbase.com";
            string FromAdressTitle = req.FromAdressTitle;
            //To Address  
            string ToAddress = req.ToAddress;
            string ToAdressTitle = req.ToAdressTitle;
            string Subject = req.Subject;
            string BodyContent = req.BodyContent;

            string SmtpServer = "smtp.zoho.com";
            //Smtp Port Number  
            int SmtpPortNumber = 465;

            try
            {
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

                    client.Connect(SmtpServer, SmtpPortNumber, true);
                    // Note: only needed if the SMTP server requires authentication  
                    // Error 5.5.1 Authentication   
                    client.Authenticate("binivarghese@expressbase.com", "1.Pappaamma");
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                }
                Console.WriteLine("The mail has been sent successfully !!");
                resp.Result = "Success By Unni";
                return resp;
            }
            catch (Exception E)
            {
                EmailError res = new EmailError();
                res.Result = E.ToJson();
                Console.WriteLine(res.Result);
                return res;

            }
         }
            

        }
}
