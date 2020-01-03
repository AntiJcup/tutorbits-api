using System;

namespace TutorBits.Video
{
    public enum TranscodingState
    {
        Unknown = 0,
        Transcoding = 1,
        Normalizing = 2,
        Finished = 3,
        Errored = 4,
        Cancelled = 5,
        Timeout = 6
    };

    public class TranscodingStateFile
    {
        public Guid ProjectId { get; set; }

        public TranscodingState State { get; set; }

        public string TranscodingOutputPath { get; set; }

        public string NormalizeOutputPath { get; set; }

        public DateTimeOffset Start { get; set; }
    }
}