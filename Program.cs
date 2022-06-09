using Ovh.Api;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Linq;

namespace ReactiveThings.DDNS
{
    class Program
    {
        //https://api.ovh.com/console/#/domain/zone/%7BzoneName%7D/record/%7Bid%7D#PUT
        static async Task Main(
            string endpoint, 
            string applicationKey,
            string applicationSecret,
            string consumerKey,
            string recordId,
            string domainName,
            string subDomainName
            )
        {
            var httpClient = new HttpClient();
            string currentIp = await GetCurrentIpAddress(httpClient);
            Console.WriteLine($"Current IP: {currentIp}");

            //https://api.ovh.com/createToken/index.cgi?GET=/me
            var ovhClient = new Client(endpoint, applicationKey, applicationSecret, consumerKey, httpClient: httpClient);
            var dnsRecord = await ovhClient.GetAsync<DnsRecord>($"/domain/zone/{domainName}/record/{recordId}");
            

            if (currentIp != dnsRecord.target)
            {
                Console.WriteLine($"IP Address has changed. Previous IP: {dnsRecord.target}");
                dnsRecord.target = $"{currentIp}";

                await ovhClient.PutAsync<DnsRecord>($"/domain/zone/{domainName}/record/{recordId}", dnsRecord);
                Console.WriteLine($"DNS OVH Record A with Id {recordId} for domain {domainName} has been updated to {currentIp}");

                await ovhClient.PostAsync($"/domain/zone/{domainName}/refresh");
                Console.WriteLine($"OVH Zone for domain {domainName} has been refreshed");
            }
            else
            {
                Console.WriteLine($"IP address has not changed");
            }
            
            var googleDnsIp = await GetDnsCurrentIpAddress(httpClient, subDomainName);
            if(currentIp != googleDnsIp)
            {
                Console.WriteLine($"Google DNS Record A for {subDomainName} is not up to date. IP: {googleDnsIp}");
            }
            else
            {
                Console.WriteLine($"Google DNS Record A for {subDomainName} is up to date");
            }
        }

        private static async Task<string> GetCurrentIpAddress(HttpClient httpClient)
        {
            var response = await httpClient.GetAsync(@"https://ipinfo.io/ip");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> GetDnsCurrentIpAddress(HttpClient httpClient, string domainName)
        {
            string requestUri = $"https://dns.google.com/resolve?name={domainName}&type=A";
            var response = await httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var googleDnsResponse = JsonSerializer.Deserialize<GoogleDnsResponse>(json);
            return googleDnsResponse.Answer?.Where(p => p.type == 1).Select(p => p.data).SingleOrDefault();
        }
    }
}
