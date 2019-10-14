using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;

namespace Tracer
{

    public abstract class TransactionLoader
    {
        public TransactionLoader()
        {
        }

        public TraceTransactionLog LoadTraceTransactionLog(TraceProject project, UInt32 partition)
        {
            TraceTransactionLog traceTransactionLog = null;
            using (var inputStream = GetTransactionLogStream(project, partition))
            {
                traceTransactionLog = TraceTransactionLog.Parser.ParseFrom(inputStream);
            }
            return traceTransactionLog;
        }

        public TraceProject LoadProject(Guid id)
        {
            TraceProject traceProject = null;
            using (var inputStream = GetProjectStream(id))
            {
                traceProject = TraceProject.Parser.ParseFrom(inputStream);
            }
            return traceProject;
        }

        protected abstract Stream GetTransactionLogStream(TraceProject project, UInt32 partition);
        protected abstract Stream GetProjectStream(Guid id);
    }
}
