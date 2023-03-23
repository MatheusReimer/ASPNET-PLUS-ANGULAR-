

using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController:BaseAPIController
    {
        private readonly DataContext _dbContext;
        public BuggyController(DataContext dbContext){
            _dbContext = dbContext;
        }
        [HttpGet("auth")]
        public ActionResult<string> GetSecret(){
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound(){
            var thing = _dbContext.Users.Find(-1);
            if(thing == null) return NotFound();
            return Ok(thing);
        }

                [HttpGet("server-error")]
        public ActionResult<string> GetServerError(){
            var thing = _dbContext.Users.Find(-1);
            var thingToReturn = thing.ToString();
            return thingToReturn;
        }

            public ActionResult<string> GetBadRequest(){
            return BadRequest("This was not a good request");
        }
    }
}