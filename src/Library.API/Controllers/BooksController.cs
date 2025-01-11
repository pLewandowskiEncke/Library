using Library.Application.Commands.CreateBook;
using Library.Application.DTOs;
using Library.Application.Queries.GetBookById;
using Library.Application.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var query = new GetBookByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<BookListDTO>> GetBooks([FromQuery] GetBooksQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook(CreateBookCommand command)
        {
            var result = await _mediator.Send(command);

            var uri = Url.Action(nameof(GetBookById), nameof(BooksController), new { id = result.Id }, Request.Scheme);

            return Created(uri, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBook(int id, [FromBody] UpdateBookCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
