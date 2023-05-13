using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class VotesController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VotesController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    [HttpPost("{username}/{votingId}/{nameId:int}")]
    public async Task<ActionResult> AddVote(string username, int votingId, int nameId)
    {
        var voting = await _unitOfWork.VotingReporitory.GetVotingByIdWithIncludes(votingId);
        var sourceUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        var name = await _unitOfWork.NameRepository.GetNameByIdWithIncludes(nameId);
        if (sourceUser.Votings.SingleOrDefault(x => x.Id == votingId) != null) 
            return BadRequest(new ApiResponse(400, "You cannot vote yourself voting" ));
        
        if (await _unitOfWork.VotesRepository.GetVoteByUserIdAndVotingId(sourceUser.Id, voting.Id) != null)
            return BadRequest(new ApiResponse(400, "You already vote this voting"));
    
        var userVote = await _unitOfWork.VotesRepository.GetUserVote(sourceUser.Id, voting.Id, name.Id);

        // if (userVote != null)
        //     return BadRequest(new ApiResponse(400, "You already vote this voting"));

        userVote = new Vote
        {
            UserId = sourceUser.Id,
            VotingId = voting.Id,
            NameId = name.Id
        };
    
        sourceUser.Votes.Add(userVote);

        voting.CountVote++;
        
        if (await _unitOfWork.Complete()) 
            return Ok();
    
        return BadRequest("Failed to like user");
    }

    [HttpGet("count/{votingId:int}")]
    public async Task<ActionResult<int>> GetCountVotes(int votingId)
    {
        var voting = await _unitOfWork.VotingReporitory.GetVotingByIdWithIncludes(votingId);

        return voting.CountVote;
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<Pagination<VoteDto>>> GetUserVotes([FromQuery] VoteParams voteParams,
        string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        voteParams.UserId = user.Id;
        
        var spec = new VoteWithSpecificationParams(voteParams);
        
        var countSpec = new VoteWithFiltersForCountSpecification(voteParams);

        var totalItems = await _unitOfWork.Repository<Vote>().CountAsync(countSpec);
        
        var votes = await _unitOfWork.Repository<Vote>().ListAsync(spec);

        var data = _mapper
            .Map<IReadOnlyList<Vote>, IReadOnlyList<VoteDto>>(votes);

        return Ok(new Pagination<VoteDto>(voteParams.PageNumber, voteParams.PageSize, totalItems, data));
    }

    [HttpGet]
    public async Task<ActionResult<Pagination<VoteDto>>> GetVotes([FromQuery] VoteParams voteParams)
    {
        var spec = new VoteWithSpecificationParams(voteParams);
        
        var countSpec = new VoteWithFiltersForCountSpecification(voteParams);

        var totalItems = await _unitOfWork.Repository<Vote>().CountAsync(countSpec);
        
        var votes = await _unitOfWork.Repository<Vote>().ListAsync(spec);

        var data = _mapper
            .Map<IReadOnlyList<Vote>, IReadOnlyList<VoteDto>>(votes);

        return Ok(new Pagination<VoteDto>(voteParams.PageNumber, voteParams.PageSize, totalItems, data));
    }

    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    // {
    //     likesParams.UserId = User.GetUserId();
    //     var users = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);
    //
    //     Response.AddPaginationHeader(users.CurrentPage,
    //         users.PageSize, users.TotalCount, users.TotalPages);
    //
    //     return Ok(users);
    // }
}