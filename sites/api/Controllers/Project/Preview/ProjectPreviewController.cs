﻿using System;
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
using Utils.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/project/preview/[action]")]
    [ApiController]
    public class ProjectPreviewController : ControllerBase
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        private readonly IConfiguration configuration_;

        public ProjectPreviewController(IConfiguration configuration, DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
            configuration_ = configuration;
        }

        [ActionName("load")]
        [HttpGet]
        public async Task<IActionResult> LoadPreview([FromQuery]Guid projectId, [FromQuery]uint offsetEnd)
        {
            try
            {
                var previewId = Guid.NewGuid().ToString();
                var project = await fileDataAccessService_.GetProject(projectId);
                if (project == null)
                {
                    return BadRequest();
                }
                await fileDataAccessService_.GeneratePreview(project, (int)offsetEnd, previewId);
                return new JsonResult(ProjectUrlGenerator.GenerateProjectPreviewUrl(previewId, projectId, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }

        [ActionName("generate")]
        [HttpPost]
        public async Task<IActionResult> GeneratePreview([FromQuery]uint offsetEnd)
        {
            try
            {
                if (Request.Body == null || Request.ContentLength <= 0)
                {
                    return BadRequest();
                }

                var previewId = Guid.NewGuid().ToString();

                var transactionLogs = TraceTransactionLogs.Parser.ParseFrom(Request.Body);
                if (transactionLogs == null || transactionLogs.CalculateSize() == 0)
                {
                    return BadRequest();
                }

                var tempID = Guid.NewGuid();
                var tempProject = new TraceProject() {
                    Id = tempID.ToString()
                };
                await fileDataAccessService_.GeneratePreview(tempProject, (int)offsetEnd, previewId, transactionLogs);
                return new JsonResult(ProjectUrlGenerator.GenerateProjectPreviewUrl(previewId, tempID, configuration_));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return BadRequest();
        }
    }
}
