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
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {

        private static readonly string url = "https://api.qasb.americanexpress.com/servicing/v1/banks/atms";
        private static readonly string apiKey = "CLIENT API KEY";

        static void Main(string[] args)
        {
            string response = null;
            string payload = "radius_unit=MI&radius=5&latitude=32.67&longitude=-96.79&limit=10&offset=1";
            var authProvider = new ApiKeyAuthProvider();
            var headers = authProvider.GenerateAuthHeaders(apiKey, null, null, null);

            var task = Task.Factory.StartNew(() => {
                using (var client = new HttpClient())
                {
                    foreach(var header in headers) 
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    response = client.GetStringAsync(url + payload).Result;            
                }
            });
            task.Wait();
            Console.WriteLine("Response: " + response);
        }
    }
}
