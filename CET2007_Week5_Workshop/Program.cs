using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public interface IObserver
        {
            void Update(NotificaitonMessage message);
            string Name { get; }
        }
        public class MemberObserver : IObserver
        {
            public string Name { get; private set; }
            private IReminderChannel reminderChannel;

            public MemberObserver (string name, IReminderChannel channel)
            {
                Name = name;
                reminderChannel = channel;
            }
            public void Update(NotificaitonMessage message)
            {
                reminderChannel.Send(Name,message);
            }
        }
        public class StaffObserver : IObserver
        {
            public string Name { get; private set; }
            private IReminderChannel reminderChannel;

            public StaffObserver(string name, IReminderChannel channel)
            {
                Name = name;
                reminderChannel = channel;
            }
            public void Update(NotificaitonMessage message)
            {
                reminderChannel.Send(Name, message);
            }
        }
        public class LibraryPortal
        {
            private IObserver[] observers;
            private int count;
            public LibraryPortal(int maxObservers)
            {
                observers = new IObserver[maxObservers];
                count = 0;
            }
            public void Register(IObserver observer)
            {
                if (count < observers.Length) 
                {
                    observers[count] = observer;
                    count++;
                }
            }
            public void BoradCast(NotificaitonMessage message)
            {
                for (int i = 0; i < count; i++)
                {
                    observers[i].Update(message);
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

            string[] recipents = { "Alice", "Bob", "Main Library" };

            for (int i = 0; i < recipents.Length; i++)
            {
                channel[i].Send(recipents[i], reminder);
            }

            Console.WriteLine("");
            Console.WriteLine("Formated Message" + reminder.ToString());

            LibraryPortal portal = new LibraryPortal(10);
            MemberObserver alice = new MemberObserver("Alice", emailchannel);
            MemberObserver bob = new MemberObserver("Bob", smschannel);
            StaffObserver john = new StaffObserver("John", noticeboardchannel);

            portal.Register(alice);
            portal.Register(bob);
            portal.Register(john);
            NotificaitonMessage reminder1 = new NotificaitonMessage()
            {
                Title = "Book Due Tommorow",
                Message = "Please return ‘Design Patterns’ by 5 PM."
            };

            portal.BoradCast(reminder1);
        }
    }
}
 