using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }
        //REGISTRAZIONE UTENTE
        [HttpPost("register")]
        // le stringhe username e password devono essere passate come oggetto, in questo caso RegisterDto
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            //prima di registrare un utente controllo se � presente nel database con il metodo UserExists()

            if (await UserExists(registerDto.Username)) return BadRequest("Username in taken");
            
            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // scrivo un metodo che restituisce un valore booleano che mi dice se lo Usermane � univoco nel database oppure no

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        //LOGIN UTENTE
        [HttpPost("login")]
        
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            //richiedo al db lo username
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
            //se è null allor anon sono registrato
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            //comparo le password per vedere se soo uguali, se lo sono allora posso accedere
            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return user;
        }


    }
}