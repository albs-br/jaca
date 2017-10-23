using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.CS;

namespace CSCompiler.Test.Unit
{
    [TestClass]
    public class ConvertTokensToMachineCodeTests
    {
        [TestMethod]
        public void Test_TokensToMachineCode_Simple_AtributionInstruction_1()
        {
            // Arrange
            var tokens = new List<Token>();
            tokens.Add(new TypeToken("byte"));
            tokens.Add(new IdentifierToken("myVar"));
            tokens.Add(new EqualToken("="));
            tokens.Add(new LiteralToken("17"));
            tokens.Add(new SemicolonToken(";"));


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);
            Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            Assert.AreEqual(1, csProgram.Commands.Count);
            Assert.AreEqual(1, csProgram.Variables.Count);
        }
    }
}
