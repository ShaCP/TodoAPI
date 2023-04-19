using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApi.Model;
using TodoApi.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            this._todoRepository = todoRepository;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
        {
            return Ok(await _todoRepository.GetAllAsync());
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(int id)
        {
            var todoItem = await _todoRepository.GetByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] TodoItem todoItem)
        {
            // if (todoItem == null)
            // {
            //     return BadRequest();
            // } 

            await _todoRepository.AddAsync(todoItem);

            return NoContent();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] TodoItem todoItem)
        {
           if (id != todoItem.Id)
            {
                return BadRequest();
            }

            await _todoRepository.UpdateAsync(todoItem);

            return NoContent();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
