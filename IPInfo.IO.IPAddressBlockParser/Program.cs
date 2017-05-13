using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace IPInfo.IO.IPAddressBlockParser
{
    class Program
    {
        private static void Main(string[] args)
        {
            // Matches the extended 4 octet ASN: https://tools.ietf.org/html/rfc4893
            if (args.Length != 1 || !Regex.IsMatch(args[0], "^AS\\d{1,5}$"))
            {
                Console.WriteLine("Please provide the ASN.\r\nExample: AS1234");
                return;
            }
            var asn = args[0];

            var blocks = GetIpBlocks(asn);

            foreach (var block in blocks)
            {
                Console.WriteLine(block);
            }
        }

        private static IEnumerable<string> GetIpBlocks(string asn)
        {
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync($"https://ipinfo.io/{asn}").GetAwaiter().GetResult(); ;
                var stream = response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();

                using (var reader = new StreamReader(stream))
                {
                    var regex = new Regex(@"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\/\d{1,2})", RegexOptions.Compiled);
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var match = regex.Match(line);
                        if (match.Success)
                            yield return match.Groups[1].Value;
                    }
                }
            }
        }
    }
}