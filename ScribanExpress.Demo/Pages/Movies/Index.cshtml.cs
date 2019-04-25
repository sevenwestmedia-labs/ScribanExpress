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
              "{{ movie.Title }}",
              "{{ movie.Title | string.truncate 10 }}",
              "{{ movie.Title.tolower | string.capitalize }}",
              "{{ movie.Title | string.truncate 10 }} </br> {{ movie.Title.tolower | string.capitalize }}",
              "{{ if movie.IsFeatured }}Featured: {{ end }}{{ movie.Title }}",
              @"{{ movie.Genres | array.join "",""   }}",
              "{{ movie | debug.ShowMembers }}"
            };

        
        }

        public IList<string> Examples { get; set; }
    }
}