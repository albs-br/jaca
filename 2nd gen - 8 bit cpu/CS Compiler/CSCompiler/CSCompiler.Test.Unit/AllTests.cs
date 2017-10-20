using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCompiler.Entities;
using CSCompiler.Entities.CS;
using CSCompiler.Entities.CS.Tokens;

namespace CSCompiler.Test.Unit
{
    [TestClass]
    public class AllTests
    {
        [TestMethod]
        public void Test_Simple_Instruction_1()
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
                var tokens = csProgram.ConvertToTokens();



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


        //Assert.AreEqual(32768, machineCodeProgram.bytes.Count);
        //Assert.AreEqual(1, machineCodeProgram.bytes[2000]);


        //[TestMethod]
        //public void TestMethod2()
        //{
        //    // Arrange
        //    var csSourceCode = " byte myVar = 17;";

        //    // Act
        //    var csProgram = new CSProgram();
        //    csProgram.SourceCodeText = csSourceCode;
        //    var machineCodeProgram = csProgram.ConvertToTokens();

        //    // Assert
        //    Assert.AreEqual(32768, machineCodeProgram.bytes.Count);
        //    Assert.AreEqual(1, machineCodeProgram.bytes[2000]);
        //}
    }
}
