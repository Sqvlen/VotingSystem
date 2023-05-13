using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class NamesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public NamesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    [HttpGet("{username}/names")]
    public async Task<ActionResult<NameDto>> GetNameByUsername(string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        var name = _mapper.Map<NameDto>(user.Name);

        return Ok(name);

    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NameDto>> GetNameById(int id)
    {
        var name = await _unitOfWork.Repository<Name>().GetByIdAsync(id);

        var nameToReturn = _mapper.Map<NameDto>(name);

        return Ok(nameToReturn);
    }
    
    [HttpGet]
    public async Task<ActionResult<Pagination<NameDto>>> GetNames([FromQuery] NameParams nameParams)
    {
        var spec = new NameWithSpecificationParams(nameParams);
        
        var countSpec = new NameWithFiltersForCountSpecification(nameParams);

        var totalItems = await _unitOfWork.Repository<Name>().CountAsync(countSpec);
        
        var users = await _unitOfWork.Repository<Name>().ListAsync(spec);

        var data = _mapper
            .Map<IReadOnlyList<Name>, IReadOnlyList<NameDto>>(users);

        return Ok(new Pagination<NameDto>(nameParams.PageNumber, nameParams.PageSize, totalItems, data));
    }
}