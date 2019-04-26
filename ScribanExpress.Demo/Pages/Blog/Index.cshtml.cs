using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScribanExpress.Demo.Pages.Blog
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public IList<string> Examples { get; set; }
    }
}