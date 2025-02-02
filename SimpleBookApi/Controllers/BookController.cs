using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SimpleBookApi.Models;

namespace SimpleBookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private static List<Book> books = new List<Book>
        {
            new Book {Id = 1, Title = "The delusion of God", Author = "John Rawlins",Year = 2006 },
            new Book {Id = 2, Title = "The Surgeon", Author = "Tess Geritsenn",Year = 2012 },
            new Book {Id = 3, Title = "Harry Potter and prisoner of Azkaban", Author = "Joan Roaling",Year = 1999 },

        };

        [HttpGet("get-all")]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            return Ok(books);
        }

        [HttpGet("get-by-id")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = books.FirstOrDefault(x => x.Id == id);
            if(book == null)
            {
                return NotFound($"The book with ID {id} not found");
            }
            return Ok(book);
        }

        [HttpPost("add-book")]
        public ActionResult<Book> AddBook([FromBody] Book book) 
        {
            book.Id = books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            return CreatedAtAction(nameof(AddBook), new {id = book.Id}, book);
        }

        [HttpPut("update-book")]
        public ActionResult<Book> UpdateBook(int id, [FromBody] Book updateBook)
        {
            var book = books.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound($"The book with ID {id} not found");
            }

            book.Title = updateBook.Title;
            book.Author = updateBook.Author;
            book.Year = updateBook.Year;

            return Ok(book);
        }

        [HttpDelete("delete-book")]
        public ActionResult<Book> DeleteBook(int id)
        {
            var book = books.FirstOrDefault(x => x.Id == id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found");
            }
            books.Remove(book);
            
            return Ok($"Book with ID {id} deleted");
        }

        [HttpGet("search-book")]
        public ActionResult<IEnumerable<Book>> FindByAuthor([FromQuery] string author)
        {
            var result = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!result.Any())
            {
                return NotFound($"Books of {author} not found");
            }
            return Ok(result);
        }
    }
}
