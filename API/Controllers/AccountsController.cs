

using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountsController:BaseAPIController
    {
        private readonly DataContext _context;
        public AccountsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO userDTO)
        {
            if (await UserExists(userDTO.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();// using keyword is used to dispose the object after use

            var user = new AppUser
            {
                UserName = userDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

            [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDTO userDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(userDTO.Username));

            if (user == null) return Unauthorized("Invalid Password or Username");

            using var hmac =  new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password));

            for(int i =0 ; i< computedHash.Length;i++){
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password or Username");
            }
            return user;
        }





        private async Task<bool> UserExists(string username){
            return await _context.Users.AnyAsync(x => x.UserName.Equals(username.ToLower()));
        }
    }
}