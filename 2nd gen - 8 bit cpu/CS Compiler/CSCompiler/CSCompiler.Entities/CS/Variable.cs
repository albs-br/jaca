using CSCompiler.Entities.Enums;
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
        public EnumVarType VarType { get; set; }


        public string GetString()
        {
            return string.Format("{0:x4}  {1,7} {2}", Address, Name, VarType.ToString().ToLower());
        }

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
