using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalara.AvaTax.Adapter.AddressService;
using Avalara.AvaTax.Adapter.TaxService;
using Avalara.AvaTax.Adapter;

namespace AvaTaxCalcSOAP
{
    class AddressTest
    {
        //will pull address out of parsed invoice object and validate it
        public static void validateAddressTest(GetTaxRequest calcreq, string AcctNum, string LicKey, string webaddr)
        {
            AddressSvc addrSvc = new AddressSvc(); //create service object and add configuration
            addrSvc.Configuration.Security.Account = AcctNum;
            addrSvc.Configuration.Security.License = LicKey;
            addrSvc.Configuration.Url = webaddr;
            addrSvc.Configuration.ViaUrl = webaddr;

            //we also need to create a validation request object
            ValidateRequest addrreq = new ValidateRequest();
            addrreq.Address = calcreq.DestinationAddress;

            try
            {
                ValidateResult addressresult = addrSvc.Validate(addrreq); //Validates a given address.
                Console.WriteLine("ValidateAddress test result: " + addressresult.ResultCode.ToString() + ", Address="
                    + addressresult.Addresses[0].Line1 + " " + addressresult.Addresses[0].City + " " + addressresult.Addresses[0].Region + " " + addressresult.Addresses[0].PostalCode);//At this point, you would display the validated result to the user for approval, and write it to the customer record.
            }
            catch (Exception ex)
            { Console.WriteLine("ValidateAddress test: Exception " + ex.Message); }
        }
    }
}
