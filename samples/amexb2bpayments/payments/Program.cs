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
        private static readonly string organizationId = "CLIENT ORGANIZATION ID";
        private static readonly string url = string.Format("https://api.qasb.americanexpress.com/b2bcommerce/v2/organizations/{0}/payments", organizationId);


       public static void Main(string[] args)
        {
            string result = null;
            string payload = "{\"payee_id\": \"OSKzWacFQ5mef07R\",\"payer_pymt_ref_id\": \"INVOICE1231\",\"payment_value\": \"892.91\",\"currency_code\": \"USD\",\"payment_due_date\": \"20160923\",\"payment_method\": \"CH\",\"check_payment\": {  \"payer_account_id\": \"AB1v6pb1eagWUkt2\",  \"check_no\": \"16556511\",  \"check_date\": \"20160923\",  \"check_memo\": \"Payment\",  \"payee_name\": \"WALMART\",  \"mail_deadline\": \"S\",  \"check_delivery_details\": {\"delivery_indicator\": \"1000\", \"mail_vendor_method_code\": \"USPS FULL\", \"mail_vendor_account_no\": \"AT7676\"  },  \"address\": [{\"id\": \"908908098\", \"type\": \"Check_Destination\", \"address_line_1\": \"3232 bell rd\", \"address_line_2\": \"SUITE 45334\", \"address_line_3\": null, \"address_line_4\": null, \"address_line_5\": null, \"city\": \"Phoenix\", \"region_code\": \"AZ\", \"region_name\": \"Arizona\", \"country_code\": \"US\", \"country_name\": \"USA\", \"zip_code\": \"85302\"}],  \"company_affiliate\": {\"id\": \"CA357656\", \"type\": \"Contact\", \"title\": \"Executive\", \"salutation\": \"Mr\", \"first_name\": \"Michael\", \"middle_name\": null, \"last_name\": \"Johnson\", \"email_id\": \"michael.johnson@this-is-a-fake-domain.com\", \"phones\": [{\"number\": \"5553490200\", \"extension\": \"40411\", \"type\": \"business\"}], \"fax_no\": \"5554344565\"  }},\"attachments\": [  {\"id\": \"Invoice1.pdf\", \"file\": \"B9DQogICAgfQ0KfQ==\"  }],\"invoices\": [  {\"id\": \"Payment1122\", \"date\": \"20160903\", \"due_date\": \"20160923\", \"payee_ref\": \"INVX00222\", \"payer_ref\": \"WORKORDER912\", \"paid_amount\": \"892.91\", \"gross_amount\": \"892.91\", \"net_value\": \"892.91\", \"tax_code\": \"3\", \"tax_value\": \"0.01\", \"freight_value\": \"0.01\", \"discount_value\": \"0.15\", \"adjustment_value\": \"0.11\", \"payer_field_1\": \"Michael\", \"payer_field_2\": null, \"payer_field_3\": null, \"payer_field_4\": null, \"short_pay_code\": \"T3\", \"short_pay_text\": \"forSOAP\", \"short_pay_value\": \"892.91\", \"additional_text\": \"for retail of SOAP\", \"line_items\": [{\"line_number\": \"121\", \"line_description\": \"for soap\", \"line_paid_value\": \"892.91\", \"purchase_order_item\": \"18\", \"part_number\": \"B200\", \"unit_price\": \"300\", \"quantity\": \"5\", \"unit_of_measure\": \"EA\", \"line_value\": \"1500\", \"purchase_order\": \"P6405\", \"tax_code\": \"3\", \"tax_jurisdiction\": \"State\", \"line_freight_value\": \"0.91\", \"line_discount_value\": \"0.18\", \"line_adjustment_value\": \"0.10\", \"payer_field_1\": \"Michael\", \"payer_field_2\": null, \"payer_field_3\": null, \"payer_field_4\": null, \"short_pay_code\": \"FE\", \"short_pay_text\": \"FreightExceedsPOMaximum\", \"short_pay_value\": \"140\"}]  }],\"risk_management_details\": {  \"user_id\": \"WAL232\",  \"title\": \"Executive\",  \"salutation\": \"Mr\",  \"first_name\": \"James\",  \"middle_name\": null,  \"last_name\": \"Philip\",  \"email_id\": \"james.philip@this-is-a-fake-domain.com\",  \"phones\": [{\"number\": \"5553490200\", \"extension\": \"404\", \"type\": \"business\"}], \"fax_no\": \"5554344565\",  \"user_count\": 0,  \"transaction_count\": 50,  \"login_volume\": 10,  \"user_tenure\": \"01/02/2015\",  \"ip_address\": \"10.235.11.11\",  \"device_id\": \"D23232323\",  \"session_id\": \"NE625256\",  \"submit_date\": \"01/02/2015 12:15:12\", \"user_agent\": {\"web_action_path\": \"Mozilla/5.0 (Windows NT 6.1; / 20100101 Firefox/ 12.0 \", \"browser_type\": \"Firefox\", \"browser_version\": \"12.0\"  },  \"payer_agent_email_ids\": [{\"payer_agent_email_id\": \"Michale@this-is-a-fake-domain.com\"}]}}";
       
            var authProvider = new HmacAuthProvider();
            var headers = authProvider.GenerateAuthHeaders(apiKey, secret, payload, url);
            var task = Task.Factory.StartNew(() => {
                using (var client = new HttpClient())
                {
                    foreach(var header in headers) 
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpResponseMessage response = client.PostAsync(url, new StringContent(payload, Encoding.UTF8, "application/vnd.amex.ace.organization.v2.hal+json")).Result;            
                    result = response.Content.ReadAsStringAsync().Result;
                }
            });
            task.Wait();
            Console.WriteLine("Response: " + result);
        }
    }
}
