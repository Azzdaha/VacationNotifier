using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace VacationNotifier
{
    internal class MainViewModel
    {
        public ObservableCollection<Division> Divisions { get; set; }
        public Division SelectedDivision { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Box2Text { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserName { get; set; }


        public MainViewModel()
        {
            Divisions = CreateDivisions();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(1);
        }

        //One can add more divisions and their corresponding mails here
        private ObservableCollection<Division> CreateDivisions()
        {
            ObservableCollection<Division> divisions = new ObservableCollection<Division>();

            divisions.Add(new Division("Elektronik", "elektronik-abteilung@gmail.com"));
            divisions.Add(new Division("Reinigung", "reinigung-abteilung@hotmail.com"));
            divisions.Add(new Division("Aldin", "aldinA369@hotmail.com"));

            return divisions;
        }

        public void SendMail()
        {
            CheckInput();

            string messageContent = $@"Sehr geehrte {SelectedDivision.Name},

Hiermit teile ich Ihnen mit, dass ich {UserName} vom {StartDate} bis zum {EndDate} im Urlaub bin.
{Box2Text}

Freundliche Grüsse
{UserName}";


            try
            {
                MailMessage message = new MailMessage();
                CreateMessage(message, messageContent);
                SmtpClient smtp = new SmtpClient();
                SetSmtpSettings(smtp);
                smtp.Send(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void SetSmtpSettings(SmtpClient smtp)
        {
            if (Email.Contains("gmail"))
            {
                smtp.Host = "smtp.gmail.com";
            }
            else if(Email.Contains("yahoo"))
            {
                smtp.Host = "smtp.mail.yahoo.com";
            }
            else
            {
                smtp.Host = "smtp.live.com";
            }

            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(Email, Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        }

        private void CreateMessage(MailMessage message, string body)
        {
            message.From = new MailAddress(Email);
            message.To.Add(new MailAddress(SelectedDivision.Mail));
            message.Subject = "Ferien";
            message.Body = body;
        }

        private void CheckInput()
        {
            //TODO write a method that checks user input (if something is empty, or dates are a mess)
        }
    }

    public class Division
    {
        public string Name { get; set; }
        public string Mail { get; set; }

        public Division(string name, string mail)
        {
            Name = name;
            Mail = mail;
        }
    }
}