/*
 * Copyright (c) 2017 American Express Travel Related Services Company, Inc.
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
        
        private static readonly string apiKey = "CLIENT API KEY";
        private static readonly string secret = "CLIENT SECRET";
        private static readonly string organizationStatusUrl = "https://api.qasb.americanexpress.com/b2bcommerce/v2/organizations/{0}/status";
	    private static readonly string accountStatusUrl = "https://api.qasb.americanexpress.com/b2bcommerce/v2/organizations/{0}/accounts/{1}/status";
	    private static readonly string paymentStatusUrl = "https://api.qasb.americanexpress.com/b2bcommerce/v2/organizations/{0}/payments/{1}/status";
	    private static readonly string organizationExceptionUrl = "https://api.qasb.americanexpress.com/b2bcommerce/v2/organizations/{0}/exceptions";

        public static void Main(string[] args)
        {
            string organizationId = "CLIENT ORGANIZATION ID";
            string accountId = "CLIENT ACCOUNT ID";
            string paymentId = "CLIENT PAYMENT ID";

            string response = InvokeResource(string.Format(organizationStatusUrl, organizationId));
            Console.WriteLine("Response from organization status resource: " + response);

            response = InvokeResource(string.Format(accountStatusUrl, organizationId, accountId));
            Console.WriteLine("Response from organization account status resource: " + response);

            response = InvokeResource(string.Format(paymentStatusUrl, organizationId, paymentId));
            Console.WriteLine("Response from payments status resource: " + response);

            response = InvokeResource(string.Format(organizationExceptionUrl, organizationId));
            Console.WriteLine("Response from organization exception resource: " + response);
        }

        private static string InvokeResource(string url) 
        {
            string result = null;
                       
            var authProvider = new HmacAuthProvider();
            var headers = authProvider.GenerateAuthHeaders(apiKey, secret, string.Empty, url, "GET");

            var task = Task.Factory.StartNew(() => {
                using (var client = new HttpClient())
                {
                    foreach(var header in headers) 
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    result = client.GetStringAsync(url).Result;
                }
            });
            task.Wait();
            return result;
        }
    }
}
