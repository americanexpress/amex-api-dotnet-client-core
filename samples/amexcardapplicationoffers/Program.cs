/*
 * Copyright (c) 2016 American Express Travel Related Services Company, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
 * in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License
 * is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions and limitations under
 * the License.
 */

using Amex.Api.Client.Core.Security.Authentication;
using System;
using System.Net.Http; 
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        private static readonly string url = "https://api.qasb.americanexpress.com/acquisition/digital/v1/applications/card_accounts/target_offers/acknowledgement_status";
        private static readonly string apiKey = "CLIENT API KEY";
        private static readonly string secret = "CLIENT SECRET";


        public static void Main(string[] args)
        {
            string result = null;
            string payload = "{\"acknowledge_offer\": {\"offer_request_id\":\"1467840166684U75512110048uMjPR8z\", \"request_timestamp\": \"2012010516024\"}}";            
            var authProvider = new HmacAuthProvider();
            var headers = authProvider.GenerateAuthHeaders(apiKey, secret, payload, url);
            var task = Task.Factory.StartNew(() => {
                using (var client = new HttpClient())
                {
                    foreach(var header in headers) 
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    Console.WriteLine("Request: " + payload);
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(payload, Encoding.UTF8, "application/json")).Result;            
                    result = response.Content.ReadAsStringAsync().Result;
                }
            });
            task.Wait();
            Console.WriteLine("Response: " + result);
        }
    }
}
