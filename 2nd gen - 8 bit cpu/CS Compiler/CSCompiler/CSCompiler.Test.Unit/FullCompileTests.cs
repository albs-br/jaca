using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCompiler.Entities.Compiler;
using System.Collections.Generic;

namespace CSCompiler.Test.Unit
{
    [TestClass]
    public class FullCompileTests
    {
        [TestMethod]
        public void Test_FullCompile_Simple_VarDefinitionInstruction_1()
        {
            // Arrange
            //var tokens = new List<Token>
            //{
            //    new TypeToken("byte"),
            //    new IdentifierToken("myVar"),
            //    new EqualToken(),
            //    new LiteralToken("17"),
            //    new SemicolonToken()
            //};
            var source = "byte myVar = 17;";


            // Act
            //var csProgram = new CSProgram();
            //var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
            var machineCodeProgram = Compiler.Compile(source);

            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] { 0x04, 0x00, 17, 0x05, 0x00, 0xce, 0x05, 0x80, 0x20, 0x2c, 0x00, 0x00 });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            var stringOutput = machineCodeProgram.GetBytesAsString(32768, expected.Count);
            Assert.AreEqual("04 00 11 05 00 ce 05 80 20 2c 00 00 ", stringOutput);

            //Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            //Assert.AreEqual(1, csProgram.Commands.Count);
            //Assert.AreEqual(1, csProgram.Variables.Count);
            //Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            //Assert.AreEqual(52768, csProgram.Variables[0].Address);
        }
    }
}
