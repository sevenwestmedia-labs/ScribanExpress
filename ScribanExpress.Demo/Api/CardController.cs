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
    public class CardController : ControllerBase
    {
        private readonly IExpressTemplateManager expressTemplateManager;

        public CardController(IExpressTemplateManager expressTemplateManager)
        {
            this.expressTemplateManager = expressTemplateManager;
        }
        [HttpGet]
        public IEnumerable<CardViewModel> Get(string titleTemplate)
        {
            var movies = new List<Movie> {
                new Movie {
                    Title = "Pet Cemetary",
                    TallImageUrl = "https://m.media-amazon.com/images/M/MV5BMTU2NDIyMDQ1Ml5BMl5BanBnXkFtZTgwNjI2OTA5NzM@._V1_SY298_SX201_AL_.jpg"
                },
                new Movie {
                    Title = "RAmy" ,
                    TallImageUrl ="https://m.media-amazon.com/images/M/MV5BMTA3NzAzNDg2NzBeQTJeQWpwZ15BbWU4MDYzNjkwOTcz._V1_SY298_SX201_AL_.jpg"
                },
                new Movie {
                    Title = "asdasdf" ,
                    TallImageUrl ="https://m.media-amazon.com/images/M/MV5BMTg3ODcxNjAzMl5BMl5BanBnXkFtZTgwODM2OTA5NzM@._V1_SY298_SX201_AL_.jpg"
                }
            };

            return movies.Select(x => CreateCardViewModel(x, titleTemplate));
        }

        private CardViewModel CreateCardViewModel(Movie movie, string titletemplate)
        {
            var movieWrapper = new { movie };
            return new CardViewModel
            {
                Title = Render(titletemplate, movieWrapper),
                Url = movie.TallImageUrl
            };
        }

        private string Render<T>(string templateText, T item) => expressTemplateManager.Render(templateText, item);
    }
}