using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuotesApi.Data;
using QuotesApi.Models;

namespace QuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {

        private QuotesDbContext _quotesDbContext;

        public QuotesController(QuotesDbContext quotesDbContext)
        {
            _quotesDbContext = quotesDbContext;
        }
        // GET: api/Quotes
        [HttpGet]
        public IActionResult Get()
        {
            //return _quotesDbContext.Quotes;
             return Ok(_quotesDbContext.Quotes); // this way will return the list with all the quotes and the status 200 ok.
            // we have the other status codes to return too. For example if we need to return NotFound (404) or BadRequest (400).

            //return StatusCode(StatusCodes.Status200OK); // In this way we can return any type of status code. By number or we can use inside the parenthesis StatusCodes. and the IDE will list to us the codes. 

        }

        // GET: api/Quotes/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
           var quote = _quotesDbContext.Quotes.Find(id);

            if(quote == null)
            {
                return NotFound("No record found ...");
            }
            else
            {
                return Ok(quote);
            }
         
        }

        // POST: api/Quotes
        [HttpPost]
        public IActionResult Post([FromBody] Quote quote)
        {
            _quotesDbContext.Quotes.Add(quote);
            _quotesDbContext.SaveChanges(); //this method saves the modifications on our database. 
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT: api/Quotes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Quote quote)
        {
            var entity = _quotesDbContext.Quotes.Find(id); //search at the database for the object that we need to modify. 

            if(entity == null)
            {
                return NotFound("No record found against this id ...");
            }
            else
            {
                entity.Title = quote.Title;
                entity.Author = quote.Author;
                entity.Description = quote.Description;
                entity.Type = quote.Type;
                entity.CreatedAt = quote.CreatedAt;
                _quotesDbContext.SaveChanges();
                return Ok("Record updated successfully...");
            }
  
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           var quote = _quotesDbContext.Quotes.Find(id);

            if(quote == null)
            {
                return NotFound("No record found against this id ...");
            }
            else
            {
                _quotesDbContext.Quotes.Remove(quote);
                _quotesDbContext.SaveChanges();
                return Ok("Quote deleted ...");
            }
            
        }
    }
}
