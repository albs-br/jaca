using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.Compiler;
using CSCompiler.Entities.CS;


namespace CSCompiler.Test.Unit
{
    [TestClass]
    public class Step3_ConvertCommandsToMachineCodeTests
    {
        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_Simple_VarDefinitionInstruction_1()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] { 0x04, 0x00, 17, 0x05, 0x00, 0xce, 0x05, 0x80, 0x20, 0x2c, 0x00, 0x00 });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            var stringOutput = machineCodeProgram.GetBytesAsString(32768, expected.Count);
            Assert.AreEqual("04 00 11 05 00 ce 05 80 20 2c 00 00 ", stringOutput);

            //Assert.AreEqual(1, csProgram.Commands.Count);
            //Assert.AreEqual(1, csProgram.Variables.Count);
            //Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            //Assert.AreEqual(52768, csProgram.Variables[0].Address);
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_Two_VarDefinitionInstructions()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] { 
                0x04, 0x00, 17, 0x05, 0x00, 0xce, 0x05, 0x80, 0x20, 0x2c, 0x00, 0x00,
                0x04, 0x00, 88, 0x05, 0x00, 0xce, 0x05, 0x80, 0x21, 0x2c, 0x00, 0x00 
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_Two_VarDefinitionInstructions_OneAtributionInstruction()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


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
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_Two_VarDefinitionInstructions_OneAtributionFromVarInstruction()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


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
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_ArithmeticInstruction_Add_1()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


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
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_ArithmeticInstruction_Add_2()
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
                new IdentifierToken("myVar1"),
                new EqualToken(),
                new LiteralToken("12"),
                new SemicolonToken(),

                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("88"),
                new SemicolonToken(),
                
                new IdentifierToken("myVar"),
                new EqualToken(),
                new IdentifierToken("myVar1"),
                new ArithmeticSignalToken("+"),
                new IdentifierToken("myVar2"),
                new SemicolonToken()
            };
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 17,     // LD A, 17     // byte myVar = 17;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x04, 0x00, 12,     // LD A, 12     // byte myVar1 = 12;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x2c, 0x00, 0x00,   // ST [HL], A
                
                0x04, 0x00, 88,     // LD A, 88     // byte myVar2 = 88;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x22,   // LD L, 0x22
                0x2c, 0x00, 0x00,   // ST [HL], A
                
                0x05, 0x00, 0xce,   // LD H, 0xce   // myVar = myVar + myVar2;
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x10, 0x00, 0x00,   // LD A, [HL]
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x22,   // LD L, 0x22
                0x12, 0x00, 0x00,   // LD C, [HL]
                0x80, 0x40, 0x00,   // ADD A, C
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_ArithmeticInstruction_Sub()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


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
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_ArithmeticInstruction_Inc()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("89"),
                new SemicolonToken(),

                new IdentifierToken("myVar"),
                new ArithmeticSignalToken("+"),
                new ArithmeticSignalToken("+"),
                new SemicolonToken()
            };
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 89,     // LD A, 89     // byte myVar = 89;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x05, 0x00, 0xce,   // LD H, 0xce   // myVar++;
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x10, 0x00, 0x00,   // LD A, [HL]
                0xa0, 0x40, 0x00,   // INC A
                0x2c, 0x00, 0x00,   // ST [HL], A
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_ArithmeticInstruction_Dec()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("89"),
                new SemicolonToken(),

                new IdentifierToken("myVar"),
                new ArithmeticSignalToken("-"),
                new ArithmeticSignalToken("-"),
                new SemicolonToken()
            };
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 89,     // LD A, 89     // byte myVar = 89;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x05, 0x00, 0xce,   // LD H, 0xce   // myVar++;
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x10, 0x00, 0x00,   // LD A, [HL]
                0xa4, 0x40, 0x00,   // DEC A
                0x2c, 0x00, 0x00,   // ST [HL], A
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_OutInstruction_WriteToLCDDisplay()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("65"),     // ASCII char 'A'
                new SemicolonToken(),

                new CommandToken("out"),
                new OpenParenthesisToken(),
                new LiteralToken("0"),
                new CommaToken(),
                new IdentifierToken("myVar"),
                new CloseParenthesisToken(),
                new SemicolonToken(),
            };
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 65,     // LD A, 65     // byte myVar = 65;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x05, 0x00, 0xce,   // LD H, 0xce   // out(0, myVar);
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x10, 0x00, 0x00,   // LD A, [HL]
                0x44, 0x00, 0x00,   // OUT 0, A
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            var stringOutput = machineCodeProgram.GetBytesAsString(32768, expected.Count);
            Assert.AreEqual("04 00 41 05 00 ce 05 80 20 2c 00 00 05 00 ce 05 80 20 10 00 00 44 00 00 ", stringOutput);
        }

        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_IfInstruction_1()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("65"),
                new SemicolonToken(),

                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("65"),
                new SemicolonToken(),

                new KeywordToken("if"),         // if(myVar == myVar2) { }
                new OpenParenthesisToken(),
                new IdentifierToken("myVar"),
                new ComparisonToken("=="),
                new IdentifierToken("myVar2"),
                new CloseParenthesisToken(),
                new OpenBracesToken(),
                new CloseBracesToken(),
            };
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 65,     // LD A, 65     // byte myVar = 65;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x04, 0x00, 65,     // LD A, 65     // byte myVar = 65;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x05, 0x00, 0xce,   // LD H, 0xce   // if(myVar == myVar2) { }
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x10, 0x00, 0x00,   // LD A, [HL]
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x12, 0x00, 0x00,   // LD C, [HL]
                0x84, 0x40, 0x00,   // SUB A, C     
                0x18, 0x80, 0x33,   // JP Z, 0x8033
                0x14, 0x80, 0x33,   // JP 0x8033    // it's the same addr because the braces are empty (no commands inside)
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void Test_ConvertCommandsToMachineCode_IfInstruction_2()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("65"),
                new SemicolonToken(),

                new TypeToken("byte"),
                new IdentifierToken("myVar2"),
                new EqualToken(),
                new LiteralToken("65"),
                new SemicolonToken(),

                new KeywordToken("if"),         // if(myVar == myVar2) { }
                new OpenParenthesisToken(),
                new IdentifierToken("myVar"),
                new ComparisonToken("=="),
                new IdentifierToken("myVar2"),
                new CloseParenthesisToken(),
                new OpenBracesToken(),

                new IdentifierToken("myVar"),
                new EqualToken(),
                new LiteralToken("27"),
                new SemicolonToken(),
                
                new CloseBracesToken(),
            };
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Act
            var machineCodeProgram = Compiler.ConvertCommandsToMachineCode(csProgram);


            // Assert
            Assert.AreEqual(65536, machineCodeProgram.Bytes.Count);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 65,     // LD A, 65     // byte myVar = 65;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x04, 0x00, 65,     // LD A, 65     // byte myVar = 65;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x2c, 0x00, 0x00,   // ST [HL], A

                0x05, 0x00, 0xce,   // LD H, 0xce   // if(myVar == myVar2) {
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x10, 0x00, 0x00,   // LD A, [HL]
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x21,   // LD L, 0x21
                0x12, 0x00, 0x00,   // LD C, [HL]
                0x84, 0x40, 0x00,   // SUB A, C     
                0x18, 0x80, 0x33,   // JP Z, 0x8033
                0x14, 0x80, 0x3f,   // JP 0x803f

                0x04, 0x00, 27,     // LD A, 27     // myVar = 27;
                0x05, 0x00, 0xce,   // LD H, 0xce
                0x05, 0x80, 0x20,   // LD L, 0x20
                0x2c, 0x00, 0x00,   // ST [HL], A
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes).GetRange(32768, expected.Count);
            CollectionAssert.AreEqual(expected, actual);

            //var stringOutput = machineCodeProgram.GetBytesAsString(32768, expected.Count);
            //Assert.AreEqual("04 00 41 05 00 ce 05 80 20 2c 00 00 05 00 ce 05 80 20 10 00 00 44 00 00 ", stringOutput);
        }
    }
}
