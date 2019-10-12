using System;

namespace Tracer
{
    public enum TraceTransactionType
    {
        CreateFile,
        DeleteFile,
        AppendFile,
        EditFile,
    }

    public class TraceTransaction
    {
        public UInt64 Offset { get; set; }

        
    }
}
