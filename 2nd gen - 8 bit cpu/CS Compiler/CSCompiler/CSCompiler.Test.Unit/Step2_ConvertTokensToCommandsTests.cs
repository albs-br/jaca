using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.Compiler;
using CSCompiler.Entities.CS;
using CSCompiler.Exceptions;

namespace CSCompiler.Test.Unit
{
    [TestClass]
    public class Step2_ConvertTokensToCommandsTests
    {
        [TestMethod]
        public void Test_ConvertTokensToCommands_Simple_VarDefinitionInstruction_1()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);

            // Assert
            Assert.AreEqual(1, csProgram.Commands.Count);
            Assert.AreEqual(5, csProgram.Commands[0].Tokens.Count);
            Assert.AreEqual(1, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_Two_VarDefinitionInstructions()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);

            // Assert
            Assert.AreEqual(2, csProgram.Commands.Count);
            Assert.AreEqual(5, csProgram.Commands[0].Tokens.Count);
            Assert.AreEqual(5, csProgram.Commands[1].Tokens.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_Two_VarDefinitionInstructions_OneAtributionInstruction()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_Two_VarDefinitionInstructions_OneAtributionFromVarInstruction()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_ArithmeticInstruction_Add_1()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_ArithmeticInstruction_Add_2()
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


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(4, csProgram.Commands.Count);
            Assert.AreEqual(3, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar1", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[2].Name);
            Assert.AreEqual(52770, csProgram.Variables[2].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_ArithmeticInstruction_Sub()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_ArithmeticInstruction_Inc()
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


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(2, csProgram.Commands.Count);
            Assert.AreEqual(1, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_ArithmeticInstruction_Dec()
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


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(2, csProgram.Commands.Count);
            Assert.AreEqual(1, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_OutInstruction_WriteToLCDDisplay()
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


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(2, csProgram.Commands.Count);
            Assert.AreEqual(1, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_IfInstruction_1()
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


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.IsInstanceOfType(csProgram.Commands[0], typeof(VarDefinitionInstruction));
            Assert.IsInstanceOfType(csProgram.Commands[1], typeof(VarDefinitionInstruction));
            Assert.IsInstanceOfType(csProgram.Commands[2], typeof(IfInstruction));
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }

        [TestMethod]
        public void Test_ConvertTokensToCommands_IfInstruction_2()
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


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);


            // Assert
            Assert.AreEqual(3, csProgram.Commands.Count);
            Assert.IsInstanceOfType(csProgram.Commands[0], typeof(VarDefinitionInstruction));
            Assert.IsInstanceOfType(csProgram.Commands[1], typeof(VarDefinitionInstruction));
            Assert.IsInstanceOfType(csProgram.Commands[2], typeof(IfInstruction));
            Assert.AreEqual(5, csProgram.Commands[0].Tokens.Count);
            Assert.AreEqual(5, csProgram.Commands[1].Tokens.Count);
            Assert.AreEqual(7, csProgram.Commands[2].Tokens.Count);
            var ifInstruction = ((IfInstruction)csProgram.Commands[2]);
            Assert.AreEqual(1, ifInstruction.InnerCommands.Count);
            Assert.IsInstanceOfType(ifInstruction.InnerCommands[0], typeof(AtributionFromLiteralInstruction));
            Assert.AreEqual(4, ifInstruction.InnerCommands[0].Tokens.Count);
            Assert.AreEqual(2, csProgram.Variables.Count);
            Assert.AreEqual("myVar", csProgram.Variables[0].Name);
            Assert.AreEqual("myVar2", csProgram.Variables[1].Name);
            Assert.AreEqual(52768, csProgram.Variables[0].Address);
            Assert.AreEqual(52769, csProgram.Variables[1].Address);
        }



        #region Exception Tests

        [TestMethod]
        [ExpectedException(typeof(UndefinedVariableException))]
        public void Test_ConvertTokensToCommands_Two_VarDefinitionInstructions_OneAtributionInstruction_UndefinedVariable_ThrowsException()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(VariableAlreadyDefinedException))]
        public void Test_ConvertTokensToCommands_Two_VarDefinitionInstructions_VariableAlreadyDefined_ThrowsException()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(VariableOutsideOfRangeException))]
        public void Test_ConvertTokensToCommands_VarDefinitionInstruction_VariableOutsideOfRange_ThrowsException()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionFormatException))]
        public void Test_ConvertTokensToCommands_InvalidInstructionFormat_ThrowsException()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionFormatException))]
        public void Test_ConvertTokensToCommands_InvalidInstructionFormat_1_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new TypeToken("byte"),
                new IdentifierToken("myVar"),
                new EqualToken()
            };


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionFormatException))]
        public void Test_ConvertTokensToCommands_InvalidInstructionFormat_2_ThrowsException()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidInstructionFormatException))]
        public void Test_ConvertTokensToCommands_InvalidInstructionFormat_3_ThrowsException()
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
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchingBracesException))]
        public void Test_ConvertTokensToCommands_UnmatchingBraces_1_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new CloseBracesToken()
            };


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchingBracesException))]
        public void Test_ConvertTokensToCommands_UnmatchingBraces_2_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new OpenBracesToken(),
                new OpenBracesToken(),
                new CloseBracesToken()
            };


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        [TestMethod]
        [ExpectedException(typeof(UnmatchingBracesException))]
        public void Test_ConvertTokensToCommands_UnmatchingBraces_3_ThrowsException()
        {
            // Arrange
            var tokens = new List<Token>
            {
                new OpenBracesToken(),
                new CloseBracesToken(),
                new CloseBracesToken()
            };


            // Act
            var csProgram = Compiler.ConvertTokensToCommands(tokens);
        }

        #endregion
    }
}
