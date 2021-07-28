using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonsHandBook.Domain.Models;
using PersonsHandBook.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using PersonsHandBook.Domain.Models.Entity;
using PersonsHandBook.Domain.Models.Input;
using PersonsHandBook.Resources.Locallizer;
using PersonsHandBook.Models;

namespace PersonsHandBook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly HandBookManager _handBookManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public PersonsController(ILoggerFactory loggerFactory, IMapper mapper, HandBookManager handBookManager , IStringLocalizer<SharedResources> stringLocalizer)
        {
            _logger = loggerFactory.CreateLogger<PersonsController>();
            _mapper = mapper;
            _handBookManager = handBookManager;
            _stringLocalizer = stringLocalizer;
        }


        [HttpGet("person/{personId}")]
        public async Task<IActionResult> Get(int personId)
        {
            var result = await _handBookManager.GetPersonById(personId);

            if (result.Success == false)
                return BadRequest(_stringLocalizer[result.ErrorMessage].Value);

            return Ok(result.Data);
           
        }

        [HttpPost("DetailSearch")]
        public IActionResult DetailSearch(DetailSearchPerson detailSearch)
        {
            var foundPersons = _handBookManager.DetailSearchPerson(detailSearch);

            if (foundPersons != null && foundPersons.Any())
                return Ok(foundPersons);

            return NotFound(_stringLocalizer[Constants.PersonNotFound].Value);
        }

        [HttpPut("EditPerson")]
        public async Task<IActionResult> EditPerson(Person person)
        {
            var result = await _handBookManager.EditPerson(person);

            return result.Success ? NoContent() : BadRequest(_stringLocalizer[result.ErrorMessage].Value);
        }

        [HttpPost("FastSearch")]
        public IActionResult SearchPersonAsync(SearchParams param)
        {
            var foundPersons = _handBookManager.FindPersonByParam(param);

            if (foundPersons != null && foundPersons.Any())
                return Ok(foundPersons);

            return NotFound(_stringLocalizer[Constants.PersonNotFound].Value);

        }

        [HttpPost("CreateNew")]
        public async Task<IActionResult> CreatePerson(CreatePersonModel createModel)
        {
            var newPerson = _mapper.Map<Person>(createModel);

            var result = await _handBookManager.CreateNewPerson(newPerson);

            return result.Success ? NoContent() : BadRequest(_stringLocalizer[result.ErrorMessage].Value);
        }

        [HttpDelete("Remove/{personId}")]
        public async Task<IActionResult> RemovePerson(int personId)
        {
            var result = await _handBookManager.RemovePerson(personId);

            return result.Success? NoContent() : BadRequest(_stringLocalizer[result.ErrorMessage].Value);
        }
        

    }
}
