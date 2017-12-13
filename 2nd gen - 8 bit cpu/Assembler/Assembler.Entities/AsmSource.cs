using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities
{
    public class AsmSource
    {
        public AsmSource(string text)
        {
            Text = text;
            Lines = new List<Line>();
            Labels = new Dictionary<string, int>();
            Variables = new Dictionary<string, int>();
            Bytes = new List<byte>();
        }

        public string Text { get; set; }
        public IList<Line> Lines { get; set; }
        public IDictionary<string, int> Labels { get; set; }
        public IDictionary<string, int> Variables { get; set; }

        public List<byte> Bytes { get; set; }

        public string BytesAsText {
            get
            {
                var bytesAsText = "";
                var counter = 0;
                foreach (var b in Bytes)
                {
                    var text = String.Format("{0:x2} ", b);
                    bytesAsText += text;

                    counter++;
                    if (counter == 3)
                    {
                        bytesAsText += Environment.NewLine;
                        counter = 0;
                    }
                }

                return bytesAsText;
            }
        }
    }
}
