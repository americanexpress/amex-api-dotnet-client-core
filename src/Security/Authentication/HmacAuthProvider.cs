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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Amex.Api.Client.Core.Security.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Amex.Api.Client.Core.Security.Authentication.BaseAuthProvider" />
    public class HmacAuthProvider : BaseAuthProvider
    {
        /// <summary>
        /// The signature format
        /// </summary>
        private static readonly string SIGNATURE_FORMAT = "{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n";

        /// <summary>
        /// The authentication header format
        /// </summary>
        private static readonly string AUTH_HEADER_FORMAT = "MAC id=\"{0}\",ts=\"{1}\",nonce=\"{2}\",bodyhash=\"{3}\",mac=\"{4}\"";

        /// <summary>
        /// Generates the authentication headers.
        /// </summary>
        /// <param name="clientKey">The client key.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="httpMethod">The HTTP method. Default value is "POST".</param>
        /// <param name="requestId">The request identifier. The value must be unique for each request.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="nonce">The nonce. The value must be unique for each request.</param>
        /// <returns></returns>
        public override ReadOnlyDictionary<string, string> GenerateAuthHeaders(string clientKey, string clientSecret, string payload, string requestUrl, string httpMethod = "POST", string requestId = null, string timestamp = null, string nonce = null)
        {
            var headers = CreateHeaderCollection(clientKey, requestId);
            Uri url = new Uri(requestUrl);
            string macAuth = GenerateMacHeader(clientKey, clientSecret, url.AbsolutePath, url.Host, url.Port, httpMethod, payload, 
                string.IsNullOrEmpty(nonce)? Guid.NewGuid().ToString() : nonce, 
                string.IsNullOrEmpty(timestamp)? CreateTimestamp() : timestamp);

            headers.Add(AuthHeaderNames.AUTHORIZATION, macAuth);
            return new ReadOnlyDictionary<string, string>(headers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientKey">Client API key.</param>
        /// <param name="clientSecret">Client secret.</param>
        /// <param name="resourcePath">Relative path to the resource.</param>
        /// <param name="host">Host name.</param>
        /// <param name="port">Port number.</param>
        /// <param name="httpMethod">HTTP method.</param>
        /// <param name="payload">Payload.</param>
        /// <param name="nonce">Unique nonce.</param>
        /// <param name="ts">Timestamp.</param>
        /// <returns></returns>
        private string GenerateMacHeader(string clientKey, string clientSecret, string resourcePath, string host, int port, string httpMethod, string payload, string nonce, string ts)
        {
            //create crypto using client secret
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret));
            hmac.Initialize();

            //body hash generation
            byte[] rawBodyHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            string bodyHash = Convert.ToBase64String(rawBodyHash);

            //create signature 
            string signature = string.Format(SIGNATURE_FORMAT, ts, nonce, httpMethod, resourcePath, host, port, bodyHash);
            byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signature));
            string signatureString = Convert.ToBase64String(signatureBytes);

            return string.Format(AUTH_HEADER_FORMAT, clientKey, ts, nonce, bodyHash, signatureString);
        }
    }
}
