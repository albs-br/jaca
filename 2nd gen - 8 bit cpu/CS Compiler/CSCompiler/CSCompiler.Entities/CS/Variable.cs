using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS
{
    public class Variable
    {
        public String Name { get; set; }
        public int Address { get; set; }
        
        // TODO:
        //public EnumVarType VarType { get; set; }

        //public int GetSize()
        //{
        //    switch (VarType)
        //    {
        //        case EnumVarType.Byte:
        //            return 1;

        //        default:
        //            break;
        //    }
        //}
    }
}
