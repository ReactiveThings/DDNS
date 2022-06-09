namespace ReactiveThings.DDNS
{
    public class DnsRecord {
        public string fieldType { get; set; }
        public int ttl { get; set; }
        public long id { get; set; }
        public string subDomain { get; set; }
        public string zone { get; set; }
        public string target { get; set; }
    }
}
