using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.CS;
using CSCompiler.Exceptions;

namespace CSCompiler.Test.Unit
{
    [TestClass]
    public class ConvertTokensToMachineCodeTests
    {
        [TestMethod]
        public void Test_TokensToMachineCode_Simple_VarDefinitionInstruction_1()
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
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
        }

        [TestMethod]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions()
        {
            // Arrange
            var tokens = new List<Token>();
            tokens.Add(new TypeToken("byte"));
            tokens.Add(new IdentifierToken("myVar"));
            tokens.Add(new EqualToken("="));
            tokens.Add(new LiteralToken("17"));
            tokens.Add(new SemicolonToken(";"));
            tokens.Add(new TypeToken("byte"));
            tokens.Add(new IdentifierToken("myVar2"));
            tokens.Add(new EqualToken("="));
            tokens.Add(new LiteralToken("88"));
            tokens.Add(new SemicolonToken(";"));


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);
            Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            Assert.AreEqual(88, machineCodeProgram.Bytes[52769]);
            Assert.AreEqual(2, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
        }

        [TestMethod]
        [ExpectedException(typeof(VariableAlreadyDefinedException))]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions_VariableAlreadyDefined_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>();
            tokens.Add(new TypeToken("byte"));
            tokens.Add(new IdentifierToken("myVar"));
            tokens.Add(new EqualToken("="));
            tokens.Add(new LiteralToken("17"));
            tokens.Add(new SemicolonToken(";"));
            tokens.Add(new TypeToken("byte"));
            tokens.Add(new IdentifierToken("myVar"));
            tokens.Add(new EqualToken("="));
            tokens.Add(new LiteralToken("199"));
            tokens.Add(new SemicolonToken(";"));


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
        }

        // Test values outside byte range
    }
}
