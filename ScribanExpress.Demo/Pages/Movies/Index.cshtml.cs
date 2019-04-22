using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScribanExpress.Demo.Models;

namespace ScribanExpress.Demo.Pages.Movies
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            Examples = new List<string>() {
              "{{ movie.Title | string.truncate 10 }} </br> {{ movie.Title.tolower | string.capitalize }}"
            };

        
        }

        public IList<string> Examples { get; set; }
    }
}