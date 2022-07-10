using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    //oggetto da restituire quando un utente accede o si registra
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
