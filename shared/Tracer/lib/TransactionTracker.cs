using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;

namespace Tracer
{

    public class TransactionTracker
    {
        public TraceProject Project { get; set; }

        //Todo change partion back to uint32
        protected UInt64 partitionOffset_ { get; set; } = 0;
        protected List<TraceTransactionLog> logs_ { get; set; }

        public TransactionTracker(UInt64 partitionSize)
        {
            Project = new TraceProject()
            {
                Id = Guid.NewGuid().ToString(),
                Duration = 0,
                PartitionSize = partitionSize
            };

            logs_ = new List<TraceTransactionLog>();
        }

        //For loading existing transactions
        //Logs should only be after the offset
        public TransactionTracker(TraceProject project, List<TraceTransactionLog> transactionLogs, UInt64 partitionOffset)
        {
            Project = project;
            partitionOffset_ = partitionOffset;
            logs_ = transactionLogs;
        }

        public TraceTransactionLog GetTransactionLogByTimeOffset(UInt64 timeOffset)
        {
            TraceTransactionLog transactionLog = null;
            var partition = Project.PartitionFromOffsetBottom(timeOffset);
            while (partition > ((UInt64)logs_.Count))
            {
                transactionLog = new TraceTransactionLog();
                transactionLog.Partition = partitionOffset_ + (UInt64)(logs_.Count - 1);
                logs_.Add(transactionLog);
            }

            if (transactionLog == null)
            {
                transactionLog = logs_[(int)partition];
            }

            return transactionLog;
        }

        protected void AddTransaction(TraceTransaction transaction)
        {
            var transactionLog = GetTransactionLogByTimeOffset(transaction.TimeOffsetMs);
            transactionLog.Transactions.Add(transaction);
            Project.Duration += transaction.TimeOffsetMs;
        }

        public void CreateFile(UInt64 timeOffset, string file_path)
        {
            var transaction = new TraceTransaction();
            transaction.Type = TraceTransaction.Types.TraceTransactionType.CreateFile;
            transaction.TimeOffsetMs = timeOffset;
            var data = new CreateFileData();
            data.FilePath = file_path;
            transaction.CreateFile = data;
        }

        public void DeleteFile(UInt64 timeOffset, string file_path)
        {
            var transaction = new TraceTransaction();
            transaction.Type = TraceTransaction.Types.TraceTransactionType.CreateFile;
            transaction.TimeOffsetMs = timeOffset;
            var data = new DeleteFileData();
            data.FilePath = file_path;
            transaction.DeleteFile = data;
        }

        public void InsertFile(UInt64 timeOffset, string file_path, UInt64 line, UInt64 offset, string insertData)
        {
            var transaction = new TraceTransaction();
            transaction.Type = TraceTransaction.Types.TraceTransactionType.CreateFile;
            transaction.TimeOffsetMs = timeOffset;
            var data = new InsertFileData();
            data.FilePath = file_path;
            data.Line = line;
            data.Offset = offset;
            data.Data = insertData;
            transaction.InsertFile = data;
        }

        public void EraseFile(UInt64 timeOffset, string file_path, UInt64 line, UInt64 offsetStart, UInt64 offsetEnd)
        {
            var transaction = new TraceTransaction();
            transaction.Type = TraceTransaction.Types.TraceTransactionType.CreateFile;
            transaction.TimeOffsetMs = timeOffset;
            var data = new EraseFileData();
            data.FilePath = file_path;
            data.Line = line;
            data.OffsetStart = offsetStart;
            data.OffsetEnd = offsetEnd;
            transaction.EraseFile = data;
        }
    }
}
