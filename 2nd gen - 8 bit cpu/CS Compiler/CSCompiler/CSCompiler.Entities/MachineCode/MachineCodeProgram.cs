using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.MachineCode
{
    public class MachineCodeProgram
    {
        // Constructor
        public MachineCodeProgram()
        {
            this.Bytes = new List<byte>(Constants.SIZE_RAM_MEMORY);

            // Initialize array filled of zeroes
            for (int i = 0; i < Constants.SIZE_RAM_MEMORY; i++)
            {
                this.Bytes.Add(0);
            }
        }

        public IList<byte> Bytes { get; set; }

        // TODO:
        //public string GetBytesAsString()
        //{
        //    var output = "";
        //    foreach (var b in bytes)
        //    {
        //        output += C
        //    }
        //}
    }
}
