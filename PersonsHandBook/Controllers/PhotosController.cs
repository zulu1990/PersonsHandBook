using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using PersonsHandBook.Domain.Models;
using PersonsHandBook.Domain.Models.Entity;
using PersonsHandBook.Infrastructure;
using PersonsHandBook.Models;
using PersonsHandBook.Resources.Locallizer;

namespace PersonsHandBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly HandBookManager _handBookManager;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;

        public PhotosController(ILoggerFactory loggerFactory, IMapper mapper, HandBookManager handBookManager, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _logger = loggerFactory.CreateLogger<PersonsController>();
            _mapper = mapper;
            _handBookManager = handBookManager;
            _stringLocalizer = stringLocalizer;
        }

        
        [HttpPost("AddPhoto")]
        public async Task<IActionResult> UploadPhoto([FromForm] UploadPhotoModel photoModel)
        {
            var photo = _mapper.Map<Photo>(photoModel);

            var result = await _handBookManager.UploadPhoto(photo, photoModel.Photo);

            return Ok();
        }


        [HttpPost("GetPhoto")]
        public async Task<IActionResult> GetPhoto(GetPhotoModel getPhotoModel)
        {
            var result = _handBookManager.GetPhoto(getPhotoModel.PersonId, getPhotoModel.ContactId);

            if (System.IO.File.Exists(result.Url))
            {
                var name = Path.GetFileName(result.Url);
                return File(await System.IO.File.ReadAllBytesAsync(result.Url), "pplication/octet-stream", name);
            }

            _logger.LogInformation("Photo Not Found");

            return NotFound(_stringLocalizer["PhotoNotFound", getPhotoModel.PersonId, getPhotoModel.ContactId].Value);
        }

        [HttpDelete("Photo/{Id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var result = await _handBookManager.DeletePhoto(id);

            if (result.Success == false)
                return BadRequest(_stringLocalizer[result.ErrorMessage]);

            return NoContent();
        }

    }
}
