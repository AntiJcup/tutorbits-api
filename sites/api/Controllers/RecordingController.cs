using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tracer;

namespace tutorbits_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordingController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public void AddTransaction()
        {
            try
            {
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
