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
using System.Threading.Tasks;

namespace Amex.Api.Client.Core.Security.Authentication
{
    public abstract class BaseAuthProvider : IAuthProvider
    {
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
        /// <returns>Read-only collection of AMEX API headers</returns>
        public abstract ReadOnlyDictionary<string, string> GenerateAuthHeaders(string clientKey, string clientSecret, string payload, string requestUrl, string httpMethod = "POST", string requestId = null, string timestamp = null, string nonce = null);

        /// <summary>
        /// Creates a timestamp in milliseconds
        /// </summary>
        /// <returns>Timestamp</returns>
        public virtual string CreateTimestamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }

        /// <summary>
        /// Creates a collection of common headers.
        /// </summary>
        /// <param name="clientKey">Client API key.</param>
        /// <param name="requestId">Unique request identifier.</param>
        /// <returns>Collection of common headers containing client API key and unique request identifier.</returns>
        public virtual Dictionary<string, string> CreateHeaderCollection(string clientKey, string requestId)
        {
            var headers = new Dictionary<string, string>();
            headers.Add(AuthHeaderNames.X_AMEX_API_KEY, clientKey);
            headers.Add(AuthHeaderNames.X_AMEX_REQUEST_ID, string.IsNullOrEmpty(requestId)? Guid.NewGuid().ToString() : requestId);
            return headers;
        }
    }
}
