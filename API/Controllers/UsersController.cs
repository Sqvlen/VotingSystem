using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<Pagination<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
    {
        var spec = new UserWithSpecificationParams(userParams);
        
        var countSpec = new UsersWithFiltersForCountSpecification(userParams);

        var totalItems = await _unitOfWork.Repository<User>().CountAsync(countSpec);
        
        var users = await _unitOfWork.Repository<User>().ListAsync(spec);

        var data = _mapper
            .Map<IReadOnlyList<User>, IReadOnlyList<MemberDto>>(users);

        return Ok(new Pagination<MemberDto>(userParams.PageNumber, userParams.PageSize, totalItems, data));
    }

    // [HttpGet("{id:int}")]
    // public async Task<ActionResult<MemberDto>> GetUserById(int id)
    // {
    //     var user = await _unitOfWork.Repository<User>().GetByIdAsync(id);
    //
    //     var userToReturn = _mapper.Map<MemberDto>(user);
    //
    //     return userToReturn;
    // }
    //
    // [HttpGet("{username}")]
    // public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
    // {
    //     var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
    //
    //     var userToReturn = _mapper.Map<MemberDto>(user);
    //
    //     return userToReturn;
    // }
}