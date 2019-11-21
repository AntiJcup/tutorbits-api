using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using GenericServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;
using TutorBits.Models.Common;
using Utils.Common;

namespace tutorbits_api.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ThumbnailController : TutorBitsController
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        public ThumbnailController(IConfiguration configuration,
                                    DBDataAccessService dbDataAccessService,
                                    FileDataAccessService fileDataAccessService)
         : base(configuration)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload([FromQuery]Guid tutorialId)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0)
                {
                    return BadRequest();
                }

                var tutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(tutorialId);
                if (tutorial == null)
                {
                    return NotFound();
                }

                if (!HasAccessToModel(tutorial))
                {
                    return Forbid(); //Only the owner and admins can modify this data
                }

                using (var memoryStream = new MemoryStream())
                {
                    await Request.Body.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    await fileDataAccessService_.CreateTutorialThumbnail(tutorialId, memoryStream);
                }
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Get([FromQuery]Guid tutorialId)
        {
            try
            {
                return new JsonResult(ProjectUrlGenerator.GenerateProjectThumbnailUrl(tutorialId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
