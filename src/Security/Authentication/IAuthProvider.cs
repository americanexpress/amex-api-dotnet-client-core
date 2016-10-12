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

using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Amex.Api.Client.Core.Security.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthProvider
    {
        /// <summary>
        /// Generates the authentication headers.
        /// </summary>
        /// <param name="clientKey">The client key.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="requestUrl">The request URL.</param>
        /// <param name="httpMethod">The HTTP method. Defualt value is "POST".</param>
        /// <param name="requestId">The request identifier. The value must be unique for each request.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="nonce">The nonce. The value must be unique for each request.</param>
        /// <returns>Read-only collection of AMEX API headers</returns>
        ReadOnlyDictionary<string, string> GenerateAuthHeaders(string clientKey, string clientSecret, string payload, string requestUrl, string httpMethod = "POST", string requestId = null, string timestamp = null, string nonce = null);
    }
}
