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
    public enum HttpMethod { POST, DELETE, PUT } 

    public class Program
    {
        private static readonly string apiKey = "CLIENT API KEY";
        private static readonly string secret = "CLIENT SECRET";
        private static readonly string url = "https://api.qasb.americanexpress.com/b2bcommerce/v2/organizations";


        public static void Main(string[] args)
        {
            string organizationId = "OBdMuB0uSCd9IJer";
            
            string orgEnrollPayload = "{\"name\": \"Johnson Company\",\"type\": \"Payee\",\"associated_payer_id\": null,\"short_name\": \"Johnson\",\"email_id\": \"John@this-is-a-fake-domain.com\",\"business_website\": \"http://www.this-is-a-fake-domain.com\",\"parent_customer_id\": null,\"addresses\": [  {\"id\": \"AFWsZurBRfD22Hca\",\"type\": \"primary\",\"address_line_1\": \"300 Park Ave\",\"address_line_2\": \"update testing\",\"address_line_3\": null,\"address_line_4\": null,\"address_line_5\": null,\"city\": \"San Jose\",\"region_code\": \"CA\",\"region_name\": \"California\",\"country_code\": \"US\",\"country_name\": \"USA\",\"zip_code\": \"95110\"  }],\"company_affiliates\": [  {\"id\": \"AFWsZurBRfD22Hca\",\"type\": \"Contact\",\"title\": \"Executive\",\"salutation\": \"Mr\",\"first_name\": \"James\",\"middle_name\": \"M\",\"last_name\": \"Philip\",\"email_id\": \"james.philip@this-is-a-fake-domain.com\",\"phones\": [  {    \"number\": \"6023490200\",    \"extension\": \"404\",    \"type\": \"business\"  }],\"fax_no\": \"5554344565\"  }],\"risk_management_details\": {  \"user_id\": \"A232\",  \"title\": \"Executive\",  \"salutation\": \"Mr\",  \"first_name\": \"James\",  \"middle_name\": \"M\",  \"last_name\": \"Philip\",  \"email_id\": \"james.philip@this-is-a-fake-domain.com\",  \"phones\": [{  \"number\": 5553490200,  \"extension\": 404,  \"type\": \"business\"}  ],  \"fax_no\": 3434344565,  \"user_count\": 50,  \"transaction_count\": 44,  \"first_transaction_date\": \"01/02/2015\",  \"login_volume\": 10,  \"user_tenure\": \"01/02/2015\",  \"avg_num_of_fx_pymts\": 50,  \"avg_fx_pymt_amt\": 5000,  \"goods_and_services\": \"Purchasing from Suppliers\",  \"ip_address\": \"10.235.11.11\",  \"device_id\": \"D23232323\",  \"session_id\": \"lit3py55t21z5v55vlm25s55\",  \"submit_date\": \"01/02/2015 12:15:12\",  \"user_agent\": {\"web_action_path\": \"Mozilla/5.0 (Windows NT 6.1) Gecko/20100101 Firefox/ 12.0 \",\"browser_type\": \"Firefox\",\"browser_version\": \"12.0\"  }}}";

            string enrollResponse = InvokeResource(url, orgEnrollPayload, HttpMethod.POST);    
            Console.WriteLine("ENROLL: " + enrollResponse);

            string orgUpdatePayload = "{ \"name\": \"Johnson Company\", \"type\": \"Payer\", \"partner_org_id\": \"abc123\", \"associated_payer_id\": null, \"doing_business_as_name\": \"ABC INC\", \"legal_name\": \"ABC\", \"short_name\": \"Johnson\", \"phone_no\": \"5553232434\", \"tax_id\": \"12345678\", \"email_id\": \"John@this-is-a-fake-domain.com\", \"business_website\": \"http://www.this-is-a-fake-domain.com\", \"parent_customer_id\": null, \"payee_fee_ind\": \"Y\", \"payee_tenure\": \"01/15/2015\", \"addresses\": [ { \"id\": \"AFWsZurBRfD22Hca\", \"type\": \"primary\", \"address_line_1\": \"300 Park Ave\", \"address_line_2\": \"update testing\", \"address_line_3\": null, \"address_line_4\": null, \"address_line_5\": null, \"city\": \"San Jose\", \"region_code\": \"CA\", \"region_name\": \"California\", \"country_code\": \"US\", \"country_name\": \"USA\", \"zip_code\": \"95110\" } ], \"company_affiliates\": [ { \"id\": \"AFWsZurBRfD22Hca\", \"type\": \"Contact\", \"title\": \"Executive\", \"salutation\": \"Mr\", \"first_name\": \"James\", \"middle_name\": \"M\", \"last_name\": \"Philip\", \"email_id\": \"james.philip@this-is-a-fake-domain.com\", \"phones\": [ { \"number\": \"5553490200\", \"extension\": \"404\", \"type\": \"business\" } ], \"fax_no\": \"5554344565\" } ]}";
            
            string updateResponse = InvokeResource(string.Format("{0}/{1}", url, organizationId), orgUpdatePayload, HttpMethod.PUT);
            Console.WriteLine("UPDATE: " + updateResponse);

            string deleteResponse = InvokeResource(string.Format("{0}/{1}", url, organizationId), string.Empty, HttpMethod.DELETE);
            Console.WriteLine("DELETE: " + deleteResponse);

        }

        private static string InvokeResource(string url, string payload, HttpMethod method) 
        {
            string result = null;
            string contentType = "application/json";           
            var authProvider = new HmacAuthProvider();
            var headers = authProvider.GenerateAuthHeaders(apiKey, secret, payload, url, method.ToString());

            var task = Task.Factory.StartNew(() => {
                using (var client = new HttpClient())
                {
                    foreach(var header in headers) 
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpResponseMessage response;
                    switch (method)
                    {
                        case HttpMethod.PUT:
                            response = client.PutAsync(url, new StringContent(payload, Encoding.UTF8, contentType)).Result;         
                            break;
                        case HttpMethod.DELETE:
                            response = client.DeleteAsync(url).Result;     
                            break;    
                        default:
                            response = client.PostAsync(url, new StringContent(payload, Encoding.UTF8, contentType)).Result;         
                            break;
                    }
                    result = string.Format("success = {0}, response = {1}", response.IsSuccessStatusCode, response.Content.ReadAsStringAsync().Result);
                }
            });
            task.Wait();
            return result;
        }
    }
}
