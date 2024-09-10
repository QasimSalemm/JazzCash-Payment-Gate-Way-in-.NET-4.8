using SampleMWALLETOTCHPCMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;

namespace SampleMWALLETOTCHPCMVC.Controllers
{
    public class MerchantController : Controller
    {
        // GET: Merchant
        public ActionResult Mobilink()
        {
            try
            { //show data on view
                MerchantModel temp = new MerchantModel();
                temp.pp_Version = "1.1";
                temp.pp_TxnType = "MWALLET";
                temp.pp_Language = "EN";
                // Use Merchant ID from JazzCash SandBox account
                temp.pp_MerchantID = "";
                temp.pp_SubMerchantID = "";
                temp.pp_DiscountedAmount = "";
                temp.pp_DiscountBank = "";
                // Use password from JazzCash SandBox account
                temp.pp_Password = "";
                temp.pp_BankID = "TBANK";
                temp.pp_ProductID = "RETL";
                temp.pp_TxnRefNo = "T" + DateTime.Now.ToString("yyyyMMddHHmmss");
                temp.pp_Amount = "5000";
                temp.pp_TxnCurrency = "PKR";
                temp.pp_TxnDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                temp.pp_BillReference = "billRef";
                temp.pp_Description = "This is a very good payment gateway";
                temp.pp_TxnExpiryDateTime = DateTime.Now.AddMinutes(30).ToString("yyyyMMddHHmmss");
                temp.pp_ReturnURL = "http://localhost/SampleMWALLETOTCHPCMVC/Merchant/Transaction";
                temp.ppmpf_1 = "1";
                temp.ppmpf_2 = "2";
                temp.ppmpf_3 = "3";
                temp.ppmpf_4 = "4";
                temp.ppmpf_5 = "5";
                // Use Integerity Salt from JazzCash SandBox account
                temp.pp_SecureHash = "";
                temp.pp_MobileNumber = "";
                temp.pp_CNIC = "";
                return View(temp);
            }

            catch (Exception e)
            {

                return View(e.Message);
            }
        }

        [HttpPost]
        public ActionResult Mobilink(FormCollection collection)
        {
            //getting data from Mobilink.cshtml view and save into inputs sortedlist
            SortedList<string, string> Inputs = new SortedList<string, string>();

            try
            {
                //we can change according to our requirement
                Inputs.Add("pp_Version", collection["pp_Version"].ToString());
                Inputs.Add("pp_TxnType", collection["pp_TxnType"].ToString());
                Inputs.Add("pp_Language", collection["pp_Language"].ToString());
                Inputs.Add("pp_MerchantID", collection["pp_MerchantID"].ToString());
                Inputs.Add("pp_SubMerchantID", collection["pp_SubMerchantID"].ToString());
                Inputs.Add("pp_DiscountedAmount", collection["pp_DiscountedAmount"].ToString());
                Inputs.Add("pp_DiscountBank", collection["pp_DiscountBank"].ToString());
                Inputs.Add("pp_Password", collection["pp_Password"].ToString());
                Inputs.Add("pp_BankID", collection["pp_BankID"].ToString());
                Inputs.Add("pp_ProductID", collection["pp_ProductID"].ToString());
                Inputs.Add("pp_TxnRefNo", collection["pp_TxnRefNo"].ToString());
                Inputs.Add("pp_Amount", collection["pp_Amount"].ToString());
                Inputs.Add("pp_TxnCurrency", collection["pp_TxnCurrency"].ToString());
                Inputs.Add("pp_TxnDateTime", collection["pp_TxnDateTime"].ToString());
                Inputs.Add("pp_BillReference", collection["pp_BillReference"].ToString());
                Inputs.Add("pp_Description", collection["pp_Description"].ToString());
                Inputs.Add("pp_TxnExpiryDateTime", collection["pp_TxnExpiryDateTime"].ToString());
                Inputs.Add("pp_ReturnURL", collection["pp_ReturnURL"].ToString());
                Inputs.Add("ppmpf_1", collection["ppmpf_1"].ToString());
                Inputs.Add("ppmpf_2", collection["ppmpf_2"].ToString());
                Inputs.Add("ppmpf_3", collection["ppmpf_3"].ToString());
                Inputs.Add("ppmpf_4", collection["ppmpf_4"].ToString());
                Inputs.Add("ppmpf_5", collection["ppmpf_5"].ToString());
                Inputs.Add("pp_MobileNumber", collection["pp_MobileNumber"].ToString());
                Inputs.Add("pp_CNIC", collection["pp_CNIC"].ToString());

                // Create a new SortedList to hold entries with non-empty values
                SortedList<string, string> filteredInputs = new SortedList<string, string>();

                // Populate the filtered list
                foreach (var entry in Inputs)
                {
                    if (!string.IsNullOrEmpty(entry.Value))
                    {
                        filteredInputs.Add(entry.Key, entry.Value);
                    }
                }
                //calculate secure hash using hmac tecnique. we can also use CalculateHash from Help.cs class
                string Hash =Helper.CalculateHMACSHA256(collection["pp_SecureHash"].ToString(), filteredInputs);
                filteredInputs["pp_SecureHash"] = Hash;

                TransactionPostDTO TransactionPost = new TransactionPostDTO();
                TransactionPost.PostParameter = filteredInputs;
                //get TransactionPostUrl from Web.config file
                TransactionPost.Url = ConfigurationManager.AppSettings["TransactionPostUrl"];
                return View("Post", TransactionPost);
            }
            catch (Exception e)
            {

                return View(e.Message);
            }
        }

        [HttpPost]
        public ActionResult Transaction(FormCollection formCollection)
        {
            //receive response from jazz cash sandbox account and show on Transactionc.cshtml
            //this view also use for return url
            Session["pp_ResponseMessage"] = formCollection["pp_ResponseMessage"];
            Session["pp_ResponseCode"] = formCollection["pp_ResponseCode"];
            Session["pp_RetreivalReferenceNo"] = formCollection["pp_RetreivalReferenceNo"];
            Session["pp_SecureHash"] = formCollection["pp_SecureHash"];
            Session["DoShow"] = false;
            return View();
        }
    }

}