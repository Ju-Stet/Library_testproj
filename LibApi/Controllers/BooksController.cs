using Lib.Domain.Core.DTOs;
using Lib.Infrastructure.LoggerService;
using Lib.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;
        private readonly ILoggerManager logger;

        public BooksController(IBookService bookService, ILoggerManager logger)
        {
            this.bookService = bookService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BookDTO>> GetByFilter([FromQuery] FilterSearchDTO model)
        {
            try
            {
                if (model == null)
                {
                    logger.LogInfo($"All books were requested in the {nameof(GetByFilter)} action");
                    return bookService.GetAll(false).ToList();
                }
                else
                    return bookService.GetByFilter(model, trackChanges: false).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong in the {nameof(GetByFilter)} action {ex}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetById(int id)
        {
            try
            {
                var bookDTO = await bookService.GetByIdAsync(id, trackChanges: false);
                if (bookDTO == null)
                {
                    logger.LogInfo($"No book found in the {nameof(GetById)} action by id {id}");

                    return NotFound();
                }
                else
                    return new ObjectResult(bookDTO);

            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong in the {nameof(GetById)} action {ex}");

                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookDTO bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    logger.LogInfo($"Incorrect info was passed to {nameof(Add)} action");

                    return BadRequest();
                }

                await bookService.AddAsync(bookDto);
                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong in the {nameof(Add)} action {ex}");

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(BookDTO bookDto)
        {
            if (bookDto == null)
                return BadRequest();

            var bookDTO = await bookService.GetByIdAsync(bookDto.Id, trackChanges: false);
            if (bookDTO == null)
                return NotFound();

            await bookService.UpdateAsync(bookDto);
            return Ok(bookDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var bookDTO = await bookService.GetByIdAsync(id, trackChanges: false);
            if (bookDTO == null)
                return NotFound();

            await bookService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}
