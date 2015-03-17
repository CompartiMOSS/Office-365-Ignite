using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WordSearchReferencesMVC.Models
{
    public class ResultItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Created { get; set; }
        public string CreatedBy { get; set; }
    }
}