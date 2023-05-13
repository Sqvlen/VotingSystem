using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<User, MemberDto>();
        CreateMap<Voting, VotingDto>();
        CreateMap<CreateVotingDto, Voting>();
        CreateMap<Name, NameDto>();
        CreateMap<NameDto, Name>();
        CreateMap<UpdateVotingDto, Voting>();
        CreateMap<Vote, VoteDto>();
    }
}