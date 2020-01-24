using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.Controllers;
using api.Controllers.Model;
using api.Models;
using api.Models.Requests;
using api.Models.Views;
using GenericServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tracer;
using TutorBits.AccountAccess;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.Models.Common;
using TutorBits.Video;
using Utils.Common;

namespace api.Controllers.Model
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VideoController : BaseModelController<Video, CreateVideoModel, UpdateVideoModel, VideoViewModel>
    {
        private readonly VideoService videoService_;

        protected override ICollection<Expression<Func<Video, object>>> DeleteIncludes
        {
            get
            {
                return new List<Expression<Func<Video, object>>>{
                    p => p.Tutorials
                };
            }
        }

        public VideoController(IConfiguration configuration,
                            DBDataAccessService dbDataAccessService,
                            AccountAccessService accountAccessService,
                            VideoService videoService)
            : base(configuration, dbDataAccessService, accountAccessService)
        {
            videoService_ = videoService;
        }

        [Authorize]
        [HttpPost]
        public virtual async Task<IActionResult> Publish([FromQuery] Guid videoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await dbDataAccessService_.GetBaseModel<Video>(null, videoId);
            if (model == null)
            {
                return NotFound(); //Update cant be called on items that dont exist
            }

            if (!HasAccessToModel(model))
            {
                return Forbid(); //Only the owner and admins can modify this data
            }

            if (model.Status != BaseState.Inactive)
            {
                return BadRequest("Unable to edit");
            }

            var transcode = await videoService_.ReadTranscodingStateFile(videoId);
            model.DurationMS = transcode.DurationMS;

            //Update model
            model.Status = BaseState.Active;
            await dbDataAccessService_.UpdateBaseModel(model);
            return Ok();
        }

        [Authorize]
        [ActionName("start")]
        [HttpPost]
        public async Task<IActionResult> StartRecordingVideo([FromQuery]Guid videoId)
        {
            try
            {
                var video = await dbDataAccessService_.GetBaseModel<Video>(null, videoId);
                if (video == null)
                {
                    return NotFound();
                }

                if (!HasAccessToModel(video))
                {
                    return Forbid(); //Only the owner and admins can modify this data
                }

                if (video.Status != BaseState.Inactive)
                {
                    return BadRequest();
                }

                var recordingId = await videoService_.StartVideoRecording(videoId);
                return new JsonResult(recordingId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [Authorize]
        [ActionName("continue")]
        [HttpPost]
        public async Task<IActionResult> ContinueRecordingVideo([FromQuery]Guid videoId, [FromQuery]string recordingId, [FromQuery]int part, [FromQuery]bool last)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0)
                {
                    return BadRequest();
                }

                return new JsonResult(await videoService_.ContinueVideoRecording(videoId, recordingId, part, Request.Body, last));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [Authorize]
        [ActionName("stop")]
        [HttpPost]
        public async Task<IActionResult> FinishRecordingVideo([FromQuery]Guid videoId, [FromQuery]string recordingId, [FromBody]ICollection<VideoPart> parts)
        {
            try
            {
                var fullVideoUrl = await videoService_.FinishVideoRecording(videoId, recordingId, parts);
                await videoService_.StartTranscoding(videoId);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [Authorize]
        [ActionName("status")]
        [HttpGet]
        public async Task<IActionResult> CheckTranscodeStatus([FromQuery]Guid videoId)
        {
            try
            {
                var status = await videoService_.CheckTranscodingStatus(videoId);
                return new JsonResult(status.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [Authorize]
        [ActionName("cancel")]
        [HttpGet]
        public async Task<IActionResult> CancelTranscoding([FromQuery]Guid videoId)
        {
            try
            {
                await videoService_.CancelTranscoding(videoId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("video")]
        [HttpGet]
        public IActionResult GetVideoUrl([FromQuery]Guid videoId)
        {
            try
            {
                return new JsonResult(ProjectUrlGenerator.GenerateProjectVideoUrl(videoId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        protected virtual async Task<bool> CanDelete(Video entity)
        {
            return !(entity.Tutorials.Any(e => e.Status == BaseState.Active));
        }
    }
}
