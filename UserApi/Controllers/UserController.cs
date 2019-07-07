using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.Models;
using UserApi.Repository;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        RepositoryContext _context;
        public UserController( RepositoryContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.User.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            return await _context.User.FindAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] User user)
        {
            if ( await Get(user.Email) == null ){
                await _context.User.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return StatusCode(409);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await Get(id);
            if ( user == null ){
                return NotFound();
            }
            _context.User.Remove(user.Value);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
