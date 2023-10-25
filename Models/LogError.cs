namespace rose_api.Models
{
    public class LogError
    {
        public int Id { get; set; }
        public string EndPoint { get; set; }
        public string Parameters { get; set; }
        public string Error { get; set; }
        public string Ip { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
