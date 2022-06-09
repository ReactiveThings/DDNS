namespace ReactiveThings.DDNS
{
    public class GoogleDnsResponse
    {
        public int Status { get; set; }
        public bool TC { get; set; }
        public bool RD { get; set; }
        public bool RA { get; set; }
        public bool AD { get; set; }
        public bool CD { get; set; }

        public GoogleDnsAnswer[] Answer { get; set; }
    }
}
