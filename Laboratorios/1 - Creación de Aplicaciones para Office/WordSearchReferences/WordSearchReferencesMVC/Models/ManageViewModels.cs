using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace WordSearchReferencesMVC.Models
{
    public class IndexViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Keyword")]
        public string Keyword { get; set; }

        public List<FileResult> Results { get; set; }
    }

    public class FileResult
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Created { get; set; }
        public string CreatedBy { get; set; }
    }

}