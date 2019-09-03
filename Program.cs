using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IPInfo.IO.IPAddressBlockParser
{
    class Program
    {
        private static readonly Regex SubnetRegex = new Regex(@"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\/\d{1,2})", RegexOptions.Compiled);

        private static async Task Main(string[] args)
        {
            // Matches the extended 4 octet ASN: https://tools.ietf.org/html/rfc4893
            if (args.Length != 1 || !Regex.IsMatch(args[0], "^AS\\d{1,5}$"))
            {
                Console.WriteLine("Please provide the ASN.\r\nExample: AS1234");
                return;
            }
            var asn = args[0];

            using (var client = new HttpClient())
            {
                var blocks = GetIpBlocks(asn, client);
                await foreach (var block in blocks)
                {
                    Console.WriteLine(block);
                }
            }
        }

        private static async IAsyncEnumerable<string> GetIpBlocks(string asn, HttpClient client)
        {
            var response = await client.GetAsync($"https://ipinfo.io/{asn}");
            var stream = await response.Content.ReadAsStreamAsync();

            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var match = SubnetRegex.Match(line);
                    if (match.Success)
                        yield return match.Groups[1].Value;
                }
            }
        }
    }
}