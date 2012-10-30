using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalara.AvaTax.Adapter;
using Avalara.AvaTax.Adapter.TaxService;
using Avalara.AvaTax.Adapter.AddressService;

namespace AvaTaxCalcSOAP
{
    class TaxTest
    {
        public static void cancelTaxTest(GetTaxRequest calcreq, string AcctNum, string LicKey, string webaddr)
        {
            TaxSvc tSvc = new TaxSvc(); //create service object and add configuration
            tSvc.Configuration.Security.Account = AcctNum;
            tSvc.Configuration.Security.License = LicKey;
            tSvc.Configuration.Url = webaddr;
            tSvc.Configuration.ViaUrl = webaddr;
            //create request object
            CancelTaxRequest cancelreq = new CancelTaxRequest();
            cancelreq.CompanyCode = calcreq.CompanyCode;
            cancelreq.DocCode = calcreq.DocCode;
            cancelreq.DocType = calcreq.DocType;
            cancelreq.CancelCode = CancelCode.DocDeleted; //this should vary according to your use case.
            try
            {
                CancelTaxResult cancelresult = tSvc.CancelTax(cancelreq); //Let's void this document to demonstrate tax/cancel
                //You would normally initiate a tax/cancel call upon voiding or deleting the document in your system.
                Console.WriteLine("CancelTax test result: " + cancelresult.ResultCode.ToString() + ", Document Voided");
                //Let's display the result of the cancellation. At this point, you would allow your system to complete the delete/void.
            }
            catch (Exception ex)
            { Console.WriteLine("CancelTax test: Exception " + ex.Message); }
        }
        public static void getTaxTest(GetTaxRequest calcreq, string AcctNum, string LicKey, string webaddr)
        {
            TaxSvc tSvc = new TaxSvc(); //create service object and add configuration
            tSvc.Configuration.Security.Account = AcctNum;
            tSvc.Configuration.Security.License = LicKey;
            tSvc.Configuration.Url = webaddr;
            tSvc.Configuration.ViaUrl = webaddr;
            try
            {
                GetTaxResult calcresult = tSvc.GetTax(calcreq); //Calculates tax on document
                Console.WriteLine("GetTax test result: " + calcresult.ResultCode.ToString() + ", " +
                "TotalTax=" + calcresult.TotalTax.ToString()); //At this point, you would write the tax calculated to your database and display to the user.
            }
            catch (Exception ex)
            { Console.WriteLine("GetTax test: Exception " + ex.Message); }
        }
        public static void postTaxTest(GetTaxRequest calcreq, string AcctNum, string LicKey, string webaddr)
        {
            TaxSvc tSvc = new TaxSvc(); //create service object and add configuration
            tSvc.Configuration.Security.Account = AcctNum;
            tSvc.Configuration.Security.License = LicKey;
            tSvc.Configuration.Url = webaddr;
            tSvc.Configuration.ViaUrl = webaddr;
            PostTaxRequest postreq = new PostTaxRequest();
            postreq.Commit = true; //will change the document state to "committed" - note that this can also be done by calling GetTax with Commit = true.
            postreq.CompanyCode = calcreq.CompanyCode;
            postreq.DocCode = calcreq.DocCode;
            postreq.DocType = calcreq.DocType;
            postreq.DocDate = calcreq.DocDate;
            //TotalAmount and TotalTax are required fields and should represent 
            //the pre-tax total amount and total calculated tax amount on the recorded transaction.
            //A mismatch will return a warning from the service, but it will still complete the PostTax.
            postreq.TotalAmount = 0; 
            postreq.TotalTax = 0;
            try
            {
                PostTaxResult postresult = tSvc.PostTax(postreq);
                Console.WriteLine("PostTax test result: " + postresult.ResultCode.ToString());
            }
            catch (Exception ex)
            { Console.WriteLine("PostTax test: Exception " + ex.Message); }
        }
        public static void getTaxHistoryTest(GetTaxRequest calcreq, string AcctNum, string LicKey, string webaddr)
        {
            TaxSvc tSvc = new TaxSvc(); //create service object and add configuration
            tSvc.Configuration.Security.Account = AcctNum;
            tSvc.Configuration.Security.License = LicKey;
            tSvc.Configuration.Url = webaddr;
            tSvc.Configuration.ViaUrl = webaddr;

            GetTaxHistoryRequest histReq = new GetTaxHistoryRequest();
            histReq.CompanyCode = calcreq.CompanyCode;
            histReq.DetailLevel = DetailLevel.Tax; //note that you can pull back a different tax detail here than was originally used on the calculation
            histReq.DocCode = calcreq.DocCode;
            histReq.DocType = calcreq.DocType;
            try {
                GetTaxHistoryResult histResult = tSvc.GetTaxHistory(histReq);
                Console.WriteLine("GetTaxHistory test result: " + histResult.ResultCode.ToString()+", "+"Tax calculated was: "+
                    histResult.GetTaxResult.TotalTax.ToString());
            }
            catch (Exception ex)
            { Console.WriteLine("GetTaxHistory test: Exception " + ex.Message); }
        }
    }
}
