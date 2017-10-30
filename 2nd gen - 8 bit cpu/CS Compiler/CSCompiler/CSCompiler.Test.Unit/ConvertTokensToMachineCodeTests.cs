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
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);


            // Assert
            Assert.AreEqual(65536,  machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] { 0x04, 0x00, 17, 0x05, 0x00, 0xce, 0x05, 0x80, 0x20, 0x2c, 0x00, 0x00});
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            var stringOutput = machineCodeProgram.GetBytesAsString(32768, expected.Count);
            Assert.AreEqual("04 00 11 05 00 ce 05 80 20 2c 00 00 ", stringOutput);

            //Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            Assert.AreEqual(1, csProgram.Commands.Count);
            Assert.AreEqual(1, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
        }

        [TestMethod]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17"),
                new SemicolonToken(),
                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("88"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] { 
                0x04, 0x00, 17, 0x05, 0x00, 0xce, 0x05, 0x80, 0x20, 0x2c, 0x00, 0x00,
                0x04, 0x00, 88, 0x05, 0x00, 0xce, 0x05, 0x80, 0x21, 0x2c, 0x00, 0x00 
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            //Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            //Assert.AreEqual(88, machineCodeProgram.Bytes[52769]);
            Assert.AreEqual(2, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions_OneAtributionInstruction()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17"),
                new SemicolonToken(),
                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("88"),
                new SemicolonToken(),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("99"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 17,
                0x05, 0x00, 0xce,
                0x05, 0x80, 0x20,
                0x2c, 0x00, 0x00,

                0x04, 0x00, 88,
                0x05, 0x00, 0xce,
                0x05, 0x80, 0x21,
                0x2c, 0x00, 0x00,

                0x04, 0x00, 99,
                0x05, 0x00, 0xce,
                0x05, 0x80, 0x21,
                0x2c, 0x00, 0x00,
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            //Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            //Assert.AreEqual(88, machineCodeProgram.Bytes[52769]);
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions_OneAtributionFromVarInstruction()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17"),
                new SemicolonToken(),
                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("88"),
                new SemicolonToken(),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new IdentifierToken("myVar2"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 17,     // LD A, 17     // byte myVar = 17;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x04, 0x00, 88,     // LD A, 88     // byte myVar2 = 88;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x2c, 0x00, 0x00,   // ST [HL], A
                
                0x05, 0x00, 0xce,   // LD H, 0xce   // myVar = myVar2;
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x10, 0x00, 0x00,   // LD A, [HL]
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            //Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            //Assert.AreEqual(88, machineCodeProgram.Bytes[52769]);
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions_OneArithmeticInstruction_1()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17"),
                new SemicolonToken(),
                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("88"),
                new SemicolonToken(),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new IdentifierToken("myVar"),
                new ArithmeticSignalToken("+"),
                new IdentifierToken("myVar2"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 17,     // LD A, 17     // byte myVar = 17;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x04, 0x00, 88,     // LD A, 88     // byte myVar2 = 88;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x2c, 0x00, 0x00,   // ST [HL], A
                
                0x05, 0x00, 0xce,   // LD H, 0xce   // myVar = myVar + myVar2;
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x10, 0x00, 0x00,   // LD A, [HL]
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x12, 0x00, 0x00,   // LD C, [HL]
                0x80, 0x40, 0x00,   // ADD A, C
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            //Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            //Assert.AreEqual(88, machineCodeProgram.Bytes[52769]);
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions_OneArithmeticInstruction_2()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("89"),
                new SemicolonToken(),
                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("88"),
                new SemicolonToken(),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new IdentifierToken("myVar"),
                new ArithmeticSignalToken("-"),
                new IdentifierToken("myVar2"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 89,     // LD A, 89     // byte myVar = 89;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x04, 0x00, 88,     // LD A, 88     // byte myVar2 = 88;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x2c, 0x00, 0x00,   // ST [HL], A
                
                0x05, 0x00, 0xce,   // LD H, 0xce   // myVar = myVar - myVar2;
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x10, 0x00, 0x00,   // LD A, [HL]
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x12, 0x00, 0x00,   // LD C, [HL]
                0x84, 0x40, 0x00,   // SUB A, C
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            //Assert.AreEqual(17, machineCodeProgram.Bytes[52768]);
            //Assert.AreEqual(88, machineCodeProgram.Bytes[52769]);
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        [ExpectedException(typeof(UndefinedVariableException))]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions_OneAtributionInstruction_UndefinedVariable_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17"),
                new SemicolonToken(),
                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("88"),
                new SemicolonToken(),
                new IdentifierToken("myVar3"),
                new EqualToken(),
                new LiteralToken("99"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(VariableAlreadyDefinedException))]
        public void Test_TokensToMachineCode_Two_VarDefinitionInstructions_VariableAlreadyDefined_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17"),
                new SemicolonToken(),
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("199"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(VariableOutsideOfRangeException))]
        public void Test_TokensToMachineCode_VarDefinitionInstruction_VariableOutsideOfRange_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("256"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionFormatException))]
        public void Test_TokensToMachineCode_InvalidInstructionFormatException_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionFormatException))]
        public void Test_TokensToMachineCode_InvalidInstructionFormatException_1_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionFormatException))]
        public void Test_TokensToMachineCode_InvalidInstructionFormatException_2_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new SemicolonToken(),
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17")
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionFormatException))]
        public void Test_TokensToMachineCode_InvalidInstructionFormatException_3_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new IdentifierToken("invalidType"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("17"),
                new SemicolonToken()
            };


            // Act
            var csProgram = new CSProgram();
            var machineCodeProgram = csProgram.ConvertTokensToMachineCode(tokens);
        }
    }
}
