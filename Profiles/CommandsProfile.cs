using AutoMapper;
using commander.Models;

namespace commander.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            CreateMap<Command, Dtos.CommandReadDto>();
        }

    }
}