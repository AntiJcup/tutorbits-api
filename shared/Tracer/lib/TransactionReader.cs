using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;

namespace Tracer
{

    public class TransactionReader<TLoader> where TLoader : TransactionLoader, new()
    {
        public TraceProject Project { get; set; }

        public TransactionReader(TraceProject project)
        {
            Project = project;
        }

        public List<TraceTransactionLog> GetTransactionLogs(UInt64 start, UInt64 end)
        {
            List<TraceTransactionLog> transactionLogs = new List<TraceTransactionLog>();
            var partitionStart = Project.PartitionFromOffsetBottom(start);
            var partitionEnd = Project.PartitionFromOffsetTop(end);

            var loader = Activator.CreateInstance(typeof(TLoader), new object[] { Project }) as TLoader;
            for (var partition = partitionStart; partition < partitionEnd; partition += Project.PartitionSize)
            {
                var transactionLog = loader.LoadTraceTransactionLog(partition);
                transactionLogs.Add(transactionLog);
            }

            return transactionLogs;
        }
    }
}
