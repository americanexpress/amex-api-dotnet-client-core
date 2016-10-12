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

namespace Amex.Api.Client.Core.Security.Authentication
{
    /// <summary>
    /// Constains authentication header property names. 
    /// </summary>
    public class AuthHeaderNames
    {
        /// <summary>
        /// The AMEX API key
        /// </summary>
        public static readonly string X_AMEX_API_KEY = "x-amex-api-key";

        /// <summary>
        /// The authorization
        /// </summary>
        public static readonly string AUTHORIZATION = "Authorization";

        /// <summary>
        /// The request identifier
        /// </summary>
        public static readonly string X_AMEX_REQUEST_ID = "x-amex-request-id";
    }
}
