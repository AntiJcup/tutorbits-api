﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Tracer;
using TutorBits.DBDataAccess;
using TutorBits.FileDataAccess;
using TutorBits.Models.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordingController : ControllerBase
    {
        private readonly DBDataAccessService dbDataAccessService_;
        private readonly FileDataAccessService fileDataAccessService_;

        public RecordingController(DBDataAccessService dbDataAccessService, FileDataAccessService fileDataAccessService)
        {
            dbDataAccessService_ = dbDataAccessService;
            fileDataAccessService_ = fileDataAccessService;
        }
        // POST api/values
        [HttpPost]
        public async Task AddTransaction()
        {
            try
            {
                var tutorial = await dbDataAccessService_.GetTutorial(Guid.NewGuid());
                var project = await fileDataAccessService_.GetProject(Guid.NewGuid());
                var transaction = TraceTransaction.Parser.ParseFrom(Request.Body);
                Console.WriteLine(transaction.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
