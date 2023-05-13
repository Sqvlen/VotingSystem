using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(IJwtTokenService jwtTokenService, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
    {
        if (_unitOfWork.UserRepository.UserExists(registerDto.Username).Result)
            return BadRequest(new ApiResponse(400, "Username is in use"));
        // new ApiValidationErrorResponse{Errors = new []{"Username is in use"}}
        
        var user = _mapper.Map<User>(registerDto);
        
        using var hmac = new HMACSHA512();
        
        user.UserName = registerDto.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordSalt = hmac.Key;

        var name = new Name
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserId = user.Id
        };

        if (name == null)
            return BadRequest(new ApiResponse(400));
        
        user.Name = name;
        
        _unitOfWork.Repository<User>().Add(user);
        await _unitOfWork.Complete();

        return new UserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Token = _jwtTokenService.CreateToken(user),
            FirstName = name.FirstName,
            LastName = name.LastName
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(loginDto.Username.ToLower());

        if (user == null)
            return Unauthorized(new ApiResponse(400, "Problem logging"));
        
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
                return Unauthorized(new ApiResponse(401, "Problem logging"));
        }

        return new UserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Token = _jwtTokenService.CreateToken(user),
            FirstName = user.Name.FirstName,
            LastName = user.Name.LastName
        };
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(HttpContext.User.Identity.Name);
    
        return new UserDto
        {
            Token = _jwtTokenService.CreateToken(user),
            Username = user.UserName
        };
    }
}