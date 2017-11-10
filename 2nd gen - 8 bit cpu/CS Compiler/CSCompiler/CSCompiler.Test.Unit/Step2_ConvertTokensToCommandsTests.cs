using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CSCompiler.Entities.CS.Tokens;
using CSCompiler.Entities.Compiler;
using CSCompiler.Entities.CS;

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

        // TODO: many tests here (copy from _old)

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
    }
}
