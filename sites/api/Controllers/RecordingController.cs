using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GenericServices;
using Microsoft.AspNetCore.Mvc;
using Tracer;
using TutorBits.Models.Common;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordingController : ControllerBase
    {
        private readonly ICrudServices _crudService;

        public RecordingController(ICrudServices crudService)
        {
            _crudService = crudService;
        }
        // POST api/values
        [HttpPost]
        public void AddTransaction()
        {
            try
            {
                var transaction = TraceTransaction.Parser.ParseFrom(Request.Body);
                Console.WriteLine(transaction.ToString());
                _crudService.ReadManyNoTracked<Tutorial>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
