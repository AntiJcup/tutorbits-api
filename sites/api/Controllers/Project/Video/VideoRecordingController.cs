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

namespace tutorbits_api.Controllers
{
    [Authorize]
    [Route("api/project/video/recording/[action]")]
    [ApiController]
    public class VideoRecordingController : TutorBitsController
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;
        private readonly LambdaAccessService lambdaAccessService_;

        public VideoRecordingController(IConfiguration configuration,
        DBDataAccessService dbDataAccessService,
        FileDataAccessService fileDataAccessService,
        LambdaAccessService lambdaAccessService)
         : base(configuration)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            lambdaAccessService_ = lambdaAccessService;
        }

        [ActionName("start")]
        [HttpPost]
        public async Task<IActionResult> StartRecordingVideo([FromQuery]Guid projectId)
        {
            try
            {
                var tutorial = await dbDataAccessService_.GetBaseModel<Tutorial>(projectId);
                if (tutorial == null)
                {
                    return NotFound();
                }

                if (!HasAccessToModel(tutorial))
                {
                    return Forbid(); //Only the owner and admins can modify this data
                }

                if (tutorial.Status != BaseState.Inactive)
                {
                    return BadRequest();
                }

                var recordingId = await fileDataAccessService_.StartVideoRecording(projectId);
                return new JsonResult(recordingId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("continue")]
        [HttpPost]
        public async Task<IActionResult> ContinueRecordingVideo([FromQuery]Guid projectId, [FromQuery]string recordingId, [FromQuery]int part, [FromQuery]bool last)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0)
                {
                    return BadRequest();
                }

                return new JsonResult(await fileDataAccessService_.ContinueVideoRecording(projectId, recordingId, part, Request.Body, last));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("stop")]
        [HttpPost]
        public async Task<IActionResult> FinishRecordingVideo([FromQuery]Guid projectId, [FromQuery]string recordingId, [FromBody]ICollection<VideoPart> parts)
        {
            try
            {
                var fullVideoUrl = await fileDataAccessService_.FinishVideoRecording(projectId, recordingId, parts);
                await lambdaAccessService_.ConvertProjectVideo(projectId);
                return new JsonResult(fullVideoUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
