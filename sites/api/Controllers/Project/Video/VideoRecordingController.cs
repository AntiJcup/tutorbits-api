using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.LambdaAccess;

namespace tutorbits_api.Controllers
{
    [Route("api/project/video/recording/[action]")]
    [ApiController]
    public class VideoRecordingController : ControllerBase
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;
        private readonly LambdaAccessService lambdaAccessService_;

        private readonly IConfiguration configuration_;

        public VideoRecordingController(IConfiguration configuration,
        DBDataAccessService dbDataAccessService,
        FileDataAccessService fileDataAccessService,
        LambdaAccessService lambdaAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            lambdaAccessService_ = lambdaAccessService;
            configuration_ = configuration;
        }

        [ActionName("start")]
        [HttpPost]
        public async Task<IActionResult> StartRecordingVideo([FromQuery]Guid projectId)
        {
            try
            {
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
        public async Task<IActionResult> ContinueRecordingVideo([FromQuery]Guid projectId, [FromQuery]string recordingId, [FromQuery]int part)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0)
                {
                    return BadRequest();
                }

                await fileDataAccessService_.ContinueVideoRecording(projectId, recordingId, part, Request.Body);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("stop")]
        [HttpPost]
        public async Task<IActionResult> FinishRecordingVideo([FromQuery]Guid projectId, [FromQuery]string recordingId)
        {
            try
            {
                var fullVideoUrl = await fileDataAccessService_.FinishVideoRecording(projectId, recordingId);
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
