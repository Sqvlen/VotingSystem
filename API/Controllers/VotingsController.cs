using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class VotingsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VotingsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<VotingDto>>> GetVotings([FromQuery] VotingParams votingParams)
    {
        var spec = new VotingWithSpecificationParams(votingParams);
        
        var countSpec = new VotingsWithFiltersForCountSpecification(votingParams);

        var totalItems = await _unitOfWork.Repository<Voting>().CountAsync(countSpec);
        
        var votings = await _unitOfWork.Repository<Voting>().ListAsync(spec);

        var data = _mapper
            .Map<IReadOnlyList<Voting>, IReadOnlyList<VotingDto>>(votings);

        return Ok(new Pagination<VotingDto>(votingParams.PageNumber, votingParams.PageSize, totalItems, data));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VotingDto>> GetVoting(int id)
    {
        var voting = await _unitOfWork.VotingReporitory.GetVotingByIdWithIncludes(id);

        var votingToReturn = _mapper.Map<VotingDto>(voting);

        return Ok(votingToReturn);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<IEnumerable<VotingDto>>> GetVotingsByUsername(string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
    
        var votings = _mapper.Map<IEnumerable<VotingDto>>(user.Votings);
    
        return Ok(votings);
    }
    
    [HttpPost("{username}/create")]
    public async Task<ActionResult> CreateVoting([FromBody] CreateVotingDto createVotingDto, string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        var voting = new Voting
        {
            Title = createVotingDto.Title,
            Details = createVotingDto.Details,
            Description = createVotingDto.Description,
            isClosed = true,
            CountVote = 0,
            Created = DateTime.Now
        };

        _unitOfWork.Repository<Voting>().Add(voting);
        user.Votings.Add(voting);
        if (await _unitOfWork.Complete())
            return Ok();

        return BadRequest(new ApiResponse(400, "Problem creating voting"));
    }

    [HttpDelete("{username}/delete/{votingId:int}")]
    public async Task<ActionResult> DeleteVoting(string username, int votingId)
    {
        var (user, voting) = await GetUserAndVoting(username, votingId);
        
        if (user.Votings.SingleOrDefault(x => x.Id == votingId) == null)
            return BadRequest(new ApiResponse(403, "This voting is not you"));

        _unitOfWork.Repository<Voting>().Delete(voting);
        user.Votings.Remove(voting);

        if (await _unitOfWork.Complete())
            return Ok();
        
        return BadRequest(new ApiResponse(400, "Problem deleting voting"));
    }

    [HttpPut("{username}/update/{votingId:int}")]
    public async Task<ActionResult> UpdateVoting([FromBody] UpdateVotingDto updateVotingDto, string username, int votingId)
    {
        var (user, voting) = await GetUserAndVoting(username, votingId);
        
        if (user.Votings.SingleOrDefault(x => x.Id == votingId) == null)
            return BadRequest(new ApiResponse(403, "This voting is not you"));

        _mapper.Map(updateVotingDto, voting);
        
        if (await _unitOfWork.Complete())
            return Ok();
        
        return BadRequest(new ApiResponse(400, "Problem updating voting"));
    }

    [HttpPost("{username}/add-name/{votingId:int}/{nameId:int}")]
    public async Task<ActionResult<VotingDto>> AddVariant(string username, int votingId, int nameId)
    {
        var (user, voting) = await GetUserAndVoting(username, votingId);
        
        if (user.Votings.SingleOrDefault(x => x.Id == votingId) == null)
            return BadRequest(new ApiResponse(403, "This voting is not you"));

        if (!voting.isClosed)
            return BadRequest(new ApiResponse(400, "This voting is not close"));

        var name = await _unitOfWork.Repository<Name>().GetByIdAsync(nameId);

        // var targetUser = _unitOfWork.UserRepository.GetUserByNameAsync(name);
        //
        // if (user.Name.FirstName != nameDto.FirstName || user.Name.LastName != nameDto.LastName ||
        //     user.Id != targetUser.Id)
        //     return BadRequest(new ApiResponse(400, "Problem adding name to voting"));

        if (voting.Names.Contains(name) || voting.Names.SingleOrDefault(x => x.Id == nameId) != null)
            return BadRequest(new ApiResponse(400, "This name has already added"));
        
        voting.Names.Add(name);
        
        if (await _unitOfWork.Complete())
            return Ok(_mapper.Map<VotingDto>(voting));
        
        return BadRequest(new ApiResponse(400, "Problem adding name to voting"));
    }

    [HttpPost("{username}/delete-name/{votingId:int}/{nameId:int}")]
    public async Task<ActionResult<VotingDto>> DeleteVariant(string username, int votingId, int nameId)
    {
        var (user, voting) = await GetUserAndVoting(username, votingId);
        
        if (user.Votings.SingleOrDefault(x => x.Id == votingId) == null)
            return BadRequest(new ApiResponse(403, "This voting is not you"));

        if (!voting.isClosed)
            return BadRequest(new ApiResponse(400, "This voting is not close"));

        var name = await _unitOfWork.Repository<Name>().GetByIdAsync(nameId);

        // var targetUser = _unitOfWork.UserRepository.GetUserByNameAsync(name);
        //
        // if (user.Name.FirstName != nameDto.FirstName || user.Name.LastName != nameDto.LastName ||
        //     user.Id != targetUser.Id)
        //     return BadRequest(new ApiResponse(400, "Problem adding name to voting"));
        
        voting.Names.Remove(name);

        if (await _unitOfWork.Complete())
            return Ok();
        
        return BadRequest(new ApiResponse(400, "Problem deleting name to voting"));
    }

    [HttpPost("{username}/publish/{votingId:int}")]
    public async Task<ActionResult> PublishVoting(string username, int votingId)
    {
        var (user, voting) = await GetUserAndVoting(username, votingId);
        
        if (user.Votings.SingleOrDefault(x => x.Id == votingId) == null)
            return BadRequest(new ApiResponse(403, "This voting is not you"));

        if (!voting.isClosed)
            return BadRequest(new ApiResponse(400, "This voting opened"));
        
        if (voting.Names.Count < 2)
            return BadRequest(new ApiResponse(400, "This voting has less than 2 names"));

        voting.isClosed = false;
        
        if (await _unitOfWork.Complete())
            return Ok();
        
        return BadRequest(new ApiResponse(400, "Problem publishing voting"));
    }

    [HttpPost("{username}/unpublish/{votingId:int}")]
    public async Task<ActionResult> UnpublishVoting(string username, int votingId)
    {
        var (user, voting) = await GetUserAndVoting(username, votingId);
        
        if (user.Votings.SingleOrDefault(x => x.Id == votingId) == null)
            return BadRequest(new ApiResponse(403, "This voting is not you"));
        
        if (voting.isClosed)
            return BadRequest(new ApiResponse(400, "This voting closed"));
        
        voting.isClosed = true;
        
        if (await _unitOfWork.Complete())
            return Ok();
        
        return BadRequest(new ApiResponse(400, "Problem unpublishing voting"));
    }


    private async Task<(User user, Voting voting)> GetUserAndVoting(string username, int votingId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        var voting = await _unitOfWork.VotingReporitory.GetVotingByIdWithIncludes(votingId);

        return (user, voting);
    }
    
    [HttpGet("{votingId:int}/names")]
    public async Task<ActionResult<Pagination<NameDto>>> GetNamesByVotingId(int votingId)
    {
        var voting = await _unitOfWork.Repository<Voting>().GetByIdAsync(votingId);

        var names = voting.Names;

        var namesToReturn = _mapper.Map<IEnumerable<NameDto>>(names);

        return Ok(namesToReturn);
    }
}