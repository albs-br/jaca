using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCompiler.Entities.CS.Tokens
{
    // State Machine
    public enum TokenStates
    {
        None,
        TypeOrIdentifierTokenStarted,
        LiteralTokenStarted,
        //TokenEnd,
    }
}
