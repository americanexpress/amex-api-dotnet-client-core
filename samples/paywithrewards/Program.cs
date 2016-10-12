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
        private static readonly string url = "https://api.qasb.americanexpress.com/risk/fraud/v1/enhanced_authorizations/online_purchases";
        private static readonly string apiKey = "CLIENT API KEY";
        private static readonly string secret = "CLIENT SECRET";


        public static void Main(string[] args)
        {
            string result = null;
            string payload = "{\"merchant_client_id\":\"Test Merchant\",\"timestamp\":\"2015-10-16T00:40:00.715-07:00\",\"card\":{\"number\":\"375987654321001\"}}";
            var authProvider = new HmacAuthProvider();
            var headers = authProvider.GenerateAuthHeaders(apiKey, secret, payload, url);
            
            var task = Task.Factory.StartNew(() => {
                using (var client = new HttpClient())
                {
                    foreach(var header in headers) 
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                    HttpResponseMessage response = client.PostAsync(url, new StringContent(payload, Encoding.UTF8, "application/json")).Result;            
                    result = response.Content.ReadAsStringAsync().Result;
                }
            });
            task.Wait();
            Console.WriteLine("Response: " + result);
        }
    }
}
