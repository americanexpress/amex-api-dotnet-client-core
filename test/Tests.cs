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

using System;
using Microsoft.Extensions.Configuration;
using Xunit;
using System.Collections.Generic;
using Amex.Api.Client.Core.Configuration;
using Amex.Api.Client.Core.Security.Authentication;
using System.Net.Http;
using System.Text;

namespace Tests
{
    public class Tests
    {
        private readonly IConfigurationRoot _config;

        public Tests()
        {
            _config = CreateConfiguration();
        }

        /// <summary>
        /// Creates the configuration.
        /// </summary>
        /// <returns></returns>
        private IConfigurationRoot CreateConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                { ConfigurationKeys.CLIENT_KEY, "UNIT-TEST-KEY-4388-87b9-85cf463231d7" },
                { ConfigurationKeys.CLIENT_SECRET, "UNIT-TEST-SEC-4206-8a21-a73eed54c896" },
                { ConfigurationKeys.BASE_URL, "https://api.qasb.americanexpress.com" }
            });
            return builder.Build();
        }

        [Theory, InlineData("/acquisition/digital/v1/applications/card_accounts/target_offers/acknowledgement_status", "{\"acknowledge_offer\": {\"offer_request_id\":	\"1467840166684U75512110048uMjPR8z\", \"request_timestamp\": \"2012010516024\"}}")]
        public async void PostWithHmac(string resourcePath, string payload)
        {
            var authProvider = new HmacAuthProvider();
            var headers = authProvider.GenerateAuthHeaders(_config[ConfigurationKeys.CLIENT_KEY], _config[ConfigurationKeys.CLIENT_SECRET], payload, _config[ConfigurationKeys.BASE_URL] + resourcePath);
            var client = new HttpClient();
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            HttpResponseMessage response = await client.PostAsync(_config[ConfigurationKeys.BASE_URL] + resourcePath, new StringContent(payload, Encoding.UTF8, "application/json"));
            var result = response.Content.ReadAsStringAsync().Result;
            Assert.True(result.ToLower().Contains("success"));
            //Console.WriteLine("Response: " + result);
        }

        [Theory, InlineData("/servicing/v1/banks/atms?", "radius_unit=MI&radius=5&latitude=32.67&longitude=-96.79&limit=10&offset=1")]
        public async void PostWithApiKey(string resourcePath, string payload)
        {
            var authProvider = new ApiKeyAuthProvider();
            var headers = authProvider.GenerateAuthHeaders(_config[ConfigurationKeys.CLIENT_KEY], _config[ConfigurationKeys.CLIENT_SECRET], null, null);
            var client = new HttpClient();
            foreach (var header in headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
            var result = await client.GetStringAsync(_config[ConfigurationKeys.BASE_URL] + resourcePath + payload);
            Assert.True(result.ToLower().Contains("atms"));
            //Console.WriteLine("Response: " + result);
        }
    }
}
