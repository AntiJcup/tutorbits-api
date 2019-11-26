using System.Text;

namespace TutorBits.FileDataAccess
{
    public class PreviewItem
    {
        public StringBuilder stringBuilder { get; set; }
        public bool isFolder { get; set; }
        public string resourcePath { get; set; }
    }
}