using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;

namespace Tracer
{
    public static class TraceProjectExtensions
    {
        public static UInt64 PartitionFromOffsetBottom(this TraceProject project, UInt64 offset)
        {
            return (offset - (offset % project.PartitionSize)) / project.PartitionSize;
        }

        public static UInt64 PartitionFromOffsetTop(this TraceProject project, UInt64 offset)
        {
            return (offset + (offset % project.PartitionSize)) / project.PartitionSize;
        }
    }
}
