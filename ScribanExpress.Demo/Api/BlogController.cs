using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScribanExpress.Abstractions;
using ScribanExpress.Demo.Models;

namespace ScribanExpress.Demo.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IExpressTemplateManager expressTemplateManager;

        public BlogController(IExpressTemplateManager expressTemplateManager)
        {
            this.expressTemplateManager = expressTemplateManager;
        }

        [HttpGet]
        public string Get(string template)
        {
            var model = new {
                Title = "My Blogs",
                Blogs = new List<Blog> {
                    new Blog {
                        Title = "Latest News",
                        PublishedDate = DateTime.Now,
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        Author = "Some Guy"
                    },
                    new Blog {
                        Title = "Not Clickbait",
                        PublishedDate = DateTime.Now,
                        Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                        Author = "jkasdfkajs"
                    }
                }
            };

            return expressTemplateManager.Render(template, model);
        }
    }
}