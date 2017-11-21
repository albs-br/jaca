using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Entities
{
    public class MachineCodeProgram
    {
        public MachineCodeProgram(string asmSource)
        {
            AsmSource = asmSource;
            Bytes = new List<byte>();
            Labels = new Dictionary<string, int>();
            Variables = new Dictionary<string, int>();
        }

        public string AsmSource { get; set; }
        public IList<byte> Bytes { get; set; }
        public string BytesAsText { get; set; }
        public IDictionary<string, int> Labels { get; set; }
        public IDictionary<string, int> Variables { get; set; }

        public IList<string> GetLines()
        {
            return AsmSource.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None
            );
        }
    }
}
