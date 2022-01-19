using AutoMapper;
using CommandService.Dtos;
using CommandService.Models;
using MessageModels;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();

        CreateMap<PlatformReadDto, Platform>()
        .ForMember(d => d.Id, opt => opt.Ignore())
        .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.Id))
        .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

        CreateMap<PlatformMessage, Platform>()
        .ForMember(d => d.Id, opt => opt.Ignore())
        .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.Id))
        .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));

        CreateMap<PlatformMessage, GenericEventDto>();


    }

}