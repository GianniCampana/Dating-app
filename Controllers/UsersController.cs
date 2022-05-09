using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        //aggiungiamo due endpoint per ottenere gli users dal db, uno generale e uno specifico

        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUsers()
        {
                var users = _context.Users.ToList();
                return users;
        }

        [HttpGet("{id}")]
        public ActionResult<AppUser> GetUsers(int id)
        {
                var user = _context.Users.Find(id);
                return user;
        }
         
    }
}