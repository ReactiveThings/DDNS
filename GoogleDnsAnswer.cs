namespace ReactiveThings.DDNS
{
    public class GoogleDnsAnswer
    {
        public string name { get; set; }
        public int type { get; set; }
        public int TTL { get; set; }
        public string data { get; set; }
    }
}
