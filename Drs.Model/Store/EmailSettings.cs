namespace Drs.Model.Store
{
    public class EmailSettings
    {
        public string Title { get; set; }
        public string TemplatePath { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Template { get; set; }
        public string Sender { get; set; }
    }
}