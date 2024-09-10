using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleMWALLETOTCHPCMVC.Models
{
    public class TransactionPostDTO
    {
        public SortedList<string, string> PostParameter { get; set; }
        public string Url { get; set; }
    }
}