using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalara.AvaTax.Adapter.TaxService;
using Avalara.AvaTax.Adapter.AddressService;
using Avalara.AvaTax.Adapter;
using System.IO;

namespace AvaTaxCalcSOAP
{
    class DocumentLoader
    {
        //This loads the invoice (or return) located at the specified path and returns a GetTaxRequest object for tax calculation.
        public static GetTaxRequest Load(string DocPath)
        {
            GetTaxRequest req = new GetTaxRequest();
            //loads the invoice file
            string[] txtInv = File.ReadAllLines(@DocPath);

            //Parses header-level data from the invoice file
            req.DocCode = txtInv[0].Split(':')[1] + DateTime.Now.ToString();
            req.CustomerCode = txtInv[1].Split(':')[1];
            req.DocDate = DateTime.Today;//txtInv[3].Split(':')[1];
            req.DocType = DocumentType.SalesInvoice;

            string[] shipto = txtInv[10].Split(':')[1].Split(',');

            //We will need to pass in two addresses, our origin and destination.
            //Parse our destination address
            Address destAddr = new Address();
            destAddr.Line1 = shipto[0];
            destAddr.City = shipto[1];
            destAddr.Region = shipto[2];
            destAddr.PostalCode = shipto[3];
            req.DestinationAddress = destAddr;

            //Hardcodes the origin address for the GetTaxRequest. This should be your warehouse or company address, and should not be hardcoded.
            Address originAddr = new Address();
            originAddr.Line1 = "PO Box 123";
            originAddr.City = "Bainbridge Island";
            originAddr.Region = "WA";
            originAddr.PostalCode = "98110";
            req.OriginAddress = originAddr;
            Line freight = new Line();
            //Pull the freight line from the header information and add to the request as an additional line item
            freight.ItemCode = "Shipping";
            freight.Qty = 1;
            freight.No = "FR";
            freight.Amount = Convert.ToDecimal(txtInv[7].Split(':')[1]);
            req.Lines.Add(freight);
            //Iterate through line items on transaction and add them to the request
            for (int i = 1; txtInv.Length > 12 + i; i++)
            {
                Line itemline = new Line();
                string[] item = txtInv[12 + i].Split(',');
                itemline.No = item[0];
                itemline.ItemCode = item[1];
                itemline.Qty = Convert.ToDouble(item[3]);
                itemline.Amount = Convert.ToDecimal(item[4]) * Convert.ToDecimal(itemline.Qty);
                req.Lines.Add(itemline);
            }



            
            return req;
        }
    }
}
