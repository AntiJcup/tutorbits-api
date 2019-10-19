using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Tracer;
using TutorBits.DataAccess;
using TutorBits.Models.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordingController : ControllerBase
    {
        private readonly DataAccessService dataAccessService_;

        public RecordingController(DataAccessService dataAccessService)
        {
            dataAccessService_ = dataAccessService;
        }
        // POST api/values
        [HttpPost]
        public async Task AddTransaction()
        {
            try
            {
                var tutorial = await dataAccessService_.GetTutorial(Guid.NewGuid());
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
