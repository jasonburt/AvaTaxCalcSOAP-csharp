using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalara.AvaTax.Adapter;
using Avalara.AvaTax.Adapter.TaxService;
using Avalara.AvaTax.Adapter.AddressService;

namespace AvaTaxCalcSOAP
{
    class Program
    {
        public const string AcctNum = ""; //this should be your Avatax Account Number e.g. 1100012345
        public const string LicKey = ""; //this should be the license key for the account above, e.g. 23CF4C53939C9725
        public const string CompanyCode = ""; //this should be the company code you set on your Admin Console, e.g. TEST
        public const string webaddr = "https://development.avalara.net";

        static void Main()
        {
            string DocPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\INV0001.txt";
            GetTaxRequest calcreq = DocumentLoader.Load(DocPath); //Loads document from file to generate request
            calcreq.CompanyCode = CompanyCode;
            //Run address validation test (address/validate) 
            //Will take an address and return a single non-ambiguous validated result or an error. 
            AddressTest.validateAddressTest(calcreq, AcctNum, LicKey, webaddr);

            //Run tax calculation test (tax/get POST)
            //Calculates tax on a transaction and, depending on parameters, records the transaction to the Admin Console.
            TaxTest.getTaxTest(calcreq, AcctNum, LicKey, webaddr);
            //Run post tax test (no REST equivalent)
            //Changes the status of a recorded transaction on the Admin Console to Posted or Committed.
            TaxTest.postTaxTest(calcreq, AcctNum, LicKey, webaddr);
            //Run get tax history test (no REST equivalent)
            //Retrieves the detail of the original request and result of a tax document stored on the Admin Console.
            TaxTest.getTaxHistoryTest(calcreq, AcctNum, LicKey, webaddr);
            //Run cancel tax test (tax/cancel)
            //Changes the status of a recorded transaction on the Admin Console to Voided.
            TaxTest.cancelTaxTest(calcreq, AcctNum, LicKey, webaddr);

            //Note that this sample does not demonstrate usage of all of the methods available in the SOAP API - 
            //just those that are most commonly used. If you are interested in a complete demonstration of the 
            //functionality available, please take a look at the AvaTaxCalcSOAP-extended sample.

            Console.WriteLine("Done");
            Console.ReadLine();
        }





    }
}
