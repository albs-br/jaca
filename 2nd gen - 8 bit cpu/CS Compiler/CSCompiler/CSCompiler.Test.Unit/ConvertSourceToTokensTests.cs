using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCompiler.Entities;
using CSCompiler.Entities.CS;
using CSCompiler.Entities.CS.Tokens;

namespace CSCompiler.Test.Unit
{
    [TestClass]
    public class ConvertSourceToTokensTests
    {
        [TestMethod]
        public void Test_SourceToTokens_Simple_VarDefinitionInstruction_1()
        {
            // Arrange
            string[] csSourceCodeArray = { 
                                        "byte myVar = 17;", 
                                        " byte myVar = 17;",        // One trailing space
                                        "   byte myVar = 17;",      // Many trailing spaces
                                        "       byte myVar = 17;",  // with Tabs
                                        "byte myVar=17;",           // without some spaces
                                        "byte myVar    =     17;",  // with some intermediary spaces
                                        "byte myVar =        17;",  // with some intermediary tabs
                                        "byte myVar = 17    ;", 
                                        "byte myVar = 17;      ", 
                                        "byte myVar = " + Environment.NewLine + "17;", 
                                    };



            foreach (string csSourceCode in csSourceCodeArray)
            {
                // Act
                var csProgram = new CSProgram();
                csProgram.SourceCodeText = csSourceCode;
                var tokens = csProgram.ConvertSourceToTokens();



                // Assert
                var errorMsg = string.Format("Error testing \"{0}\"", csSourceCode);

                Assert.AreEqual(5, tokens.Count, errorMsg);

                Assert.IsInstanceOfType(tokens[0], typeof(TypeToken), errorMsg);
                Assert.IsInstanceOfType(tokens[1], typeof(IdentifierToken), errorMsg);
                Assert.IsInstanceOfType(tokens[2], typeof(EqualToken), errorMsg);
                Assert.IsInstanceOfType(tokens[3], typeof(LiteralToken), errorMsg);
                Assert.IsInstanceOfType(tokens[4], typeof(SemicolonToken), errorMsg);

                Assert.AreEqual("byte", tokens[0].Text, errorMsg);
                Assert.AreEqual("myVar", tokens[1].Text, errorMsg);
                Assert.AreEqual("=", tokens[2].Text, errorMsg);
                Assert.AreEqual("17", tokens[3].Text, errorMsg);
                Assert.AreEqual(";", tokens[4].Text, errorMsg);
            }
        }

        [TestMethod]
        public void Test_SourceToTokens_Simple_AtributionInstruction_1()
        {
            // Arrange
            string[] csSourceCodeArray = {
                                        "myVar = 208;",
                                        " myVar = 208;",        // One trailing space
                                        "   myVar = 208;",      // Many trailing spaces
                                        "       myVar = 208;",  // with Tabs
                                        "myVar=208;",           // without some spaces
                                        "myVar    =     208;",  // with some intermediary spaces
                                        "myVar =        208;",  // with some intermediary tabs
                                        "myVar = 208    ;",
                                        "myVar = 208;      ",
                                        "myVar = " + Environment.NewLine + "208;",
                                    };



            foreach (string csSourceCode in csSourceCodeArray)
            {
                // Act
                var csProgram = new CSProgram();
                csProgram.SourceCodeText = csSourceCode;
                var tokens = csProgram.ConvertSourceToTokens();



                // Assert
                var errorMsg = string.Format("Error testing \"{0}\"", csSourceCode);

                Assert.AreEqual(4, tokens.Count, errorMsg);

                Assert.IsInstanceOfType(tokens[0], typeof(IdentifierToken), errorMsg);
                Assert.IsInstanceOfType(tokens[1], typeof(EqualToken), errorMsg);
                Assert.IsInstanceOfType(tokens[2], typeof(LiteralToken), errorMsg);
                Assert.IsInstanceOfType(tokens[3], typeof(SemicolonToken), errorMsg);

                Assert.AreEqual("myVar", tokens[0].Text, errorMsg);
                Assert.AreEqual("=", tokens[1].Text, errorMsg);
                Assert.AreEqual("208", tokens[2].Text, errorMsg);
                Assert.AreEqual(";", tokens[3].Text, errorMsg);
            }
        }

        [TestMethod]
        public void Test_SourceToTokens_Simple_AtributionInstruction_2()
        {
            // Arrange
            string[] csSourceCodeArray = {
                                        "myVar = myVar2;",
                                        " myVar = myVar2;",        // One trailing space
                                        "   myVar = myVar2;",      // Many trailing spaces
                                        "       myVar = myVar2;",  // with Tabs
                                        "myVar=myVar2;",           // without some spaces
                                        "myVar    =     myVar2;",  // with some intermediary spaces
                                        "myVar =        myVar2;",  // with some intermediary tabs
                                        "myVar = myVar2    ;",
                                        "myVar = myVar2;      ",
                                        "myVar = " + Environment.NewLine + "myVar2;",
                                    };



            foreach (string csSourceCode in csSourceCodeArray)
            {
                // Act
                var csProgram = new CSProgram();
                csProgram.SourceCodeText = csSourceCode;
                var tokens = csProgram.ConvertSourceToTokens();



                // Assert
                var errorMsg = string.Format("Error testing \"{0}\"", csSourceCode);

                Assert.AreEqual(4, tokens.Count, errorMsg);

                Assert.IsInstanceOfType(tokens[0], typeof(IdentifierToken), errorMsg);
                Assert.IsInstanceOfType(tokens[1], typeof(EqualToken), errorMsg);
                Assert.IsInstanceOfType(tokens[2], typeof(IdentifierToken), errorMsg);
                Assert.IsInstanceOfType(tokens[3], typeof(SemicolonToken), errorMsg);

                Assert.AreEqual("myVar", tokens[0].Text, errorMsg);
                Assert.AreEqual("=", tokens[1].Text, errorMsg);
                Assert.AreEqual("myVar2", tokens[2].Text, errorMsg);
                Assert.AreEqual(";", tokens[3].Text, errorMsg);
            }
        }

        [TestMethod]
        public void Test_SourceToTokens_Simple_ArithmeticInstruction_1()
        {
            // Arrange
            string[] csSourceCodeArray = {
                                        "myVar = 208 + 4;",
                                        " myVar = 208 + 4;",        // One trailing space
                                        "   myVar = 208 + 4;",      // Many trailing spaces
                                        "       myVar = 208 + 4;",  // with Tabs
                                        "myVar=208+4;",           // without some spaces
                                        "myVar    =     208 +  4;",  // with some intermediary spaces
                                        "myVar =        208 + 4;",  // with some intermediary tabs
                                        "myVar = 208    + 4 ;",
                                        "myVar = 208 + 4;      ",
                                        "myVar = " + Environment.NewLine + "208 + 4;",
                                    };



            foreach (string csSourceCode in csSourceCodeArray)
            {
                // Act
                var csProgram = new CSProgram();
                csProgram.SourceCodeText = csSourceCode;
                var tokens = csProgram.ConvertSourceToTokens();



                // Assert
                var errorMsg = string.Format("Error testing \"{0}\"", csSourceCode);

                Assert.AreEqual(6, tokens.Count, errorMsg);

                Assert.IsInstanceOfType(tokens[0], typeof(IdentifierToken), errorMsg);
                Assert.IsInstanceOfType(tokens[1], typeof(EqualToken), errorMsg);
                Assert.IsInstanceOfType(tokens[2], typeof(LiteralToken), errorMsg);
                Assert.IsInstanceOfType(tokens[3], typeof(ArithmeticSignalToken), errorMsg);
                Assert.IsInstanceOfType(tokens[4], typeof(LiteralToken), errorMsg);
                Assert.IsInstanceOfType(tokens[5], typeof(SemicolonToken), errorMsg);

                Assert.AreEqual("myVar", tokens[0].Text, errorMsg);
                Assert.AreEqual("=", tokens[1].Text, errorMsg);
                Assert.AreEqual("208", tokens[2].Text, errorMsg);
                Assert.AreEqual("+", tokens[3].Text, errorMsg);
                Assert.AreEqual("4", tokens[4].Text, errorMsg);
                Assert.AreEqual(";", tokens[5].Text, errorMsg);
            }
        }

        [TestMethod]
        public void Test_SourceToTokens_Simple_ArithmeticInstruction_2()
        {
            // Arrange
            string[] csSourceCodeArray = {
                                        "myVar = 208 - otherVar;",
                                        " myVar = 208 - otherVar;",        // One trailing space
                                        "   myVar = 208 - otherVar;",      // Many trailing spaces
                                        "       myVar = 208 - otherVar;",  // with Tabs
                                        "myVar=208-otherVar;",           // without some spaces
                                        "myVar    =     208 -  otherVar;",  // with some intermediary spaces
                                        "myVar =        208 - otherVar;",  // with some intermediary tabs
                                        "myVar = 208    - otherVar ;",
                                        "myVar = 208 - otherVar;      ",
                                        "myVar = " + Environment.NewLine + "208 - otherVar;",
                                    };



            foreach (string csSourceCode in csSourceCodeArray)
            {
                // Act
                var csProgram = new CSProgram();
                csProgram.SourceCodeText = csSourceCode;
                var tokens = csProgram.ConvertSourceToTokens();



                // Assert
                var errorMsg = string.Format("Error testing \"{0}\"", csSourceCode);

                Assert.AreEqual(6, tokens.Count, errorMsg);

                Assert.IsInstanceOfType(tokens[0], typeof(IdentifierToken), errorMsg);
                Assert.IsInstanceOfType(tokens[1], typeof(EqualToken), errorMsg);
                Assert.IsInstanceOfType(tokens[2], typeof(LiteralToken), errorMsg);
                Assert.IsInstanceOfType(tokens[3], typeof(ArithmeticSignalToken), errorMsg);
                Assert.IsInstanceOfType(tokens[4], typeof(IdentifierToken), errorMsg);
                Assert.IsInstanceOfType(tokens[5], typeof(SemicolonToken), errorMsg);

                Assert.AreEqual("myVar", tokens[0].Text, errorMsg);
                Assert.AreEqual("=", tokens[1].Text, errorMsg);
                Assert.AreEqual("208", tokens[2].Text, errorMsg);
                Assert.AreEqual("-", tokens[3].Text, errorMsg);
                Assert.AreEqual("otherVar", tokens[4].Text, errorMsg);
                Assert.AreEqual(";", tokens[5].Text, errorMsg);
            }
        }

        [TestMethod]
        public void Test_SourceToTokens_VarDefinitionInstruction_InvalidType()
        {
            // Arrange
            string csSourceCode = "invalidType myVar = 208;";

            // Act
            var csProgram = new CSProgram();
            csProgram.SourceCodeText = csSourceCode;
            var tokens = csProgram.ConvertSourceToTokens();

            // Assert
            Assert.AreEqual(5, tokens.Count);

            Assert.IsInstanceOfType(tokens[0], typeof(IdentifierToken));
            Assert.IsInstanceOfType(tokens[1], typeof(IdentifierToken));
            Assert.IsInstanceOfType(tokens[2], typeof(EqualToken));
            Assert.IsInstanceOfType(tokens[3], typeof(LiteralToken));
            Assert.IsInstanceOfType(tokens[4], typeof(SemicolonToken));

            Assert.AreEqual("invalidType", tokens[0].Text);
            Assert.AreEqual("myVar", tokens[1].Text);
            Assert.AreEqual("=", tokens[2].Text);
            Assert.AreEqual("208", tokens[3].Text);
            Assert.AreEqual(";", tokens[4].Text);
        }
    }
}
