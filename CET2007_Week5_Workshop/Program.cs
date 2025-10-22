using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET2007_Week5_Workshop
{
    internal class Program
    {
        public class NotificaitonMessage
        {
            public string Title { get; set; }
            public string Message { get; set; }

            public override string ToString()
            {
                return $"Reminder: {Title} - {Message}";
            }
        }
        public interface IReminderChannel
        {
            void Send(string recipient, NotificaitonMessage Message );
        }
        public class EmailChannel : IReminderChannel
        {
            public void Send(string recipient, NotificaitonMessage Message )
            {
                Console.WriteLine($"Email sent to {recipient} : {Message.Title} - {Message.Message}");
            }
        }
        public class SmsChannel : IReminderChannel
        {
            public void Send (string recipient, NotificaitonMessage Message )
            {
                Console.WriteLine($"SMS sent to {recipient} : {Message.Title} - {Message.Message}");
            }
        }
        public class NoticeBoardChannel : IReminderChannel
        {
            public void Send(string recipient, NotificaitonMessage Message)
            {
                Console.WriteLine($"Notice posted at {recipient} : {Message.Title} - {Message.Message}");
            }
        }
        public class ReminderFactory
        {
            public  IReminderChannel CreateReminder (string choice)
            {
                if (choice.ToLower() == "email")
                {
                    return new EmailChannel();
                }
                else if (choice.ToLower() == "sms")
                {
                     return new SmsChannel();
                }
                else if (choice.ToLower() == "noticeboard")
                {
                    return new NoticeBoardChannel();    
                }
                else
                {
                    throw new ArgumentException("Invalid notification type" + choice);
                }    
            }
                
        }
        static void Main(string[] args)
        {
            ReminderFactory Factory = new ReminderFactory();

            IReminderChannel emailchannel = Factory.CreateReminder("email");
            IReminderChannel smschannel = Factory.CreateReminder("sms");
            IReminderChannel noticeboardchannel = Factory.CreateReminder("noticeboard");

            NotificaitonMessage reminder = new NotificaitonMessage()
            {
                Title = "Book due soon",
                Message = "Please return 'Object-Oriented Design' by Friday 5pm "
            };

            IReminderChannel [] channel = {emailchannel, smschannel, noticeboardchannel};

            string[] recipents = { "Alice", "Bob", "Main Library Foyer" };

            for (int i = 0; i < recipents.Length; i++)
            {
                channel[i].Send(recipents[i], reminder);
            }

            Console.WriteLine("");
            Console.WriteLine("Formated Message" + reminder.ToString());

            
        }
    }
}
