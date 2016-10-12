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
            string payload = "{\"timestamp\":\"2013-12-13T11:10:00.715-05:00\",\"transaction_data\":{\"card_number\":\"375987654321001\",\"amount\":\"175.25\",\"timestamp\":\"2013-12-13T11:10:00.715-05:00\",\"currency_code\":\"840\",\"card_acceptor_id\":\"1030026553\",\"is_coupon_used\":\"false\",\"electronic_delivery_email\":\"12user@xyz.com\",\"top5_items_in_cart\":\"00010002000300040005\",\"merchant_product_sku\":\"TKDC315U\",\"shipping_method\":\"02\",\"number_of_gift_cards_in_cart\":\"2\"},\"purchaser_information\":{\"customer_email\":\"customer@wal.com\",\"billing_address\":\"1234 Main Street\",\"billing_postal_code\":\"12345\",\"billing_first_name\":\"Test\",\"billing_last_name\":\"User\",\"billing_phone_number\":\"6028888888\",\"shipto_address\":\"1234 Main Street\",\"shipto_postal_code\":\"12345\",\"shipto_first_name\":\"Test\",\"shipto_last_name\":\"User\",\"shipto_phone_number\":\"6028888888\",\"shipto_country_code\":\"840\",\"latitude_of_customers_device\":\"38.897683\",\"longitude_of_customers_device\":\"-77.036497\",\"device_id\":\"123456789012345678901234567890123456\",\"device_type\":\"01\",\"device_timezone\":\"UTC-07:00\",\"device_ip\":\"10.0.0.0\",\"host_name\":\"PHX.QW.AOL.COM\",\"user_agent\":\"Mozilla\",\"customer_ani\":\"\",\"customer_II_digits\":\"11\"},\"registration_details\":{\"is_registered\":\"true\",\"registered_name\":\"John Smith\",\"registered_email\":\"12user@abc.com\",\"registered_postal_code\":\"123456\",\"registered_address\":\"4712 Good Road\",\"registered_phone\":\"6027777777\",\"count_of_shipto_addresses_on_file\":\"03\",\"registered_account_tenure\":\"720\"},\"registration_details_change_history\":{\"is_registration_updated\":\"1\",\"registered_name\":\"36500\",\"registered_email\":\"1\",\"registered_password\":\"0000036500\",\"registered_postal_code\":\"36500\",\"registered_address\":\"0000036500\",\"registered_phone\":\"0000036500\",\"shipto_address\":\"0000036500\",\"shipto_name\":\"0000036500\"},\"seller_information\":{\"latitude\":\"38.897683\",\"longitude\":\"-77.036497\",\"owner_name\":\"Iam Owner\",\"seller_id\":\"1234567890\",\"business_name\":\"TestITD\",\"tenure\":\"36\",\"transaction_type_indicator\":\"\",\"address\":\"123 Main Street\",\"phone\":\"6021111111\",\"email\":\"user@lmn.com\",\"postal_code\":\"45678\",\"region\":\"USA\",\"country_code\":\"840\"}}";            
            
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
