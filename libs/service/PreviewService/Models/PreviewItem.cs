using System.Text;

namespace TutorBits.Preview
{
    public class PreviewItem
    {
        public StringBuilder stringBuilder { get; set; }
        public bool isFolder { get; set; }
        public string resourcePath { get; set; }

        public string resourceId { get; set; }
    }
}