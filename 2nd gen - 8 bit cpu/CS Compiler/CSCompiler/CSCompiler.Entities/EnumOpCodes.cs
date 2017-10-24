using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities
{
    public enum EnumOpCodes
    {
        NoOp = 0,
        LD_R1_data = 1,
        LD_R1_R2 = 2,
        LD_R1_addr = 3,
        // TODO: complete
    }
}
