using System.Collections.Generic;
using AutoMapper;
using commander.Data;
using commander.Dtos;
using commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace commander.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //private readonly MockCommanderRepo _repository = new MockCommanderRepo();
        //Get api/commands
        [HttpGet]
        public ActionResult <IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }
        //GET api/commands/{id}
        [HttpGet("{id}", Name="GetCommandById")]
        public ActionResult <CommandReadDto> GetCommandById (int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            return NotFound();
        }

        //POST api/commands
        [HttpPost]
        public ActionResult <CommandReadDto> CreateCommand(CommandCreateDto payload)
        {
            // variable is a mapping between DTO payload and the model
            var commandModel = _mapper.Map<Command>(payload);
            // CreateCommand refers to the reposiotry, and it takes the mapping between dto payload and model
            _repository.CreateCommand(commandModel);
            // Saves changes to SQL database
            _repository.SaveChanges();

            // passes the model just created from mapping the payload to the create dto and reads it back through the read dto
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            //return Ok(commandReadDto); this was old way, the createdatroute returns a 201 and a uri
            return CreatedAtRoute(nameof(GetCommandById), new {id = commandReadDto.Id}, commandReadDto);

        }

        //PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto payload)
        {
            // Loads the model with info from the database
            var commandModelFromRepo = _repository.GetCommandById(id);
            // validates there was something for the id number
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            // maps the payload to the database model
            _mapper.Map(payload, commandModelFromRepo);

            // actually don't technically need to do this (good practice in case you switch out EF)
            _repository.UpdateCommand(commandModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }
    }
}