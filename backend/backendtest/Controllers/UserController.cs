using backendtest.Data;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;
[Route("api/stock")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationContext _context;
    public UserController(ApplicationContext context)
    {
     _context = context;   
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }
}