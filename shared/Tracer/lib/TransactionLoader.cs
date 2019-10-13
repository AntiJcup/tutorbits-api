using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;

namespace Tracer
{

    public abstract class TransactionLoader
    {
        public TraceProject Project { get; set; }

        public TransactionLoader(TraceProject project)
        {
            Project = project;
        }

        public TraceTransactionLog LoadTraceTransactionLog(UInt64 partition)
        {
            TraceTransactionLog traceTransactionLog = null;
            using (var inputStream = GetTransactionLogStream(partition))
            {
                TraceTransactionLog.Parser.ParseFrom(inputStream);
            }
            return traceTransactionLog;
        }

        protected abstract Stream GetTransactionLogStream(UInt64 partition);
    }
}
