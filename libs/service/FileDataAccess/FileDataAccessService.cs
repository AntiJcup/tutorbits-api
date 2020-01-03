using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Tracer;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Text;
using TutorBits.Models.Common;
using System.IO.Compression;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TutorBits.FileDataAccess
{
    public class FileDataAccessService
    {

        private readonly FileDataLayerInterface dataLayer_;

        private readonly IConfiguration configuration_;

        public FileDataAccessService(IConfiguration configuration, FileDataLayerInterface dataLayer)
        {
            configuration_ = configuration;
            dataLayer_ = dataLayer;

        }
    }
}
