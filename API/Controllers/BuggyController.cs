using API.Errors;
using Core.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly DataBaseContext _dataBaseContext;

    public BuggyController(DataBaseContext dataBaseContext)
    {
        _dataBaseContext = dataBaseContext;
    }
    
    [HttpGet("testauth")]
    [Authorize]
    public ActionResult<string> GetSecretText()
    {
        return "secret stuff";
    }
    
    [HttpGet("not-found")]
    public ActionResult GetNotFoundRequest()
    {
        var temp = _dataBaseContext.Users.Find(1000);
        if (temp == null)
            return NotFound(new ApiResponse(404));
        
        return Ok();
    }

    [HttpGet("server-error")]
    public ActionResult GetServerErrorRequest()
    {
        var temp = _dataBaseContext.Users.Find(1000);
        
        var tempToReturn = temp.ToString();

        return Ok();
    }
    
    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }

    [HttpGet("badrequest/{id:int}")]
    public ActionResult GetNotFoundRequest(int id)
    {
        return BadRequest(new ApiResponse(400));
    }
}