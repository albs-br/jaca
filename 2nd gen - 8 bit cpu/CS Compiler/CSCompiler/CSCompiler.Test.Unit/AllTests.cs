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
        public void TestMethod1()
        {
            // Arrange
            var csSourceCode = "byte myVar = 17;";

            // Act
            var csProgram = new CSProgram();
            csProgram.SourceCodeText = csSourceCode;
            var tokens = csProgram.ConvertToTokens();

            // Assert
            Assert.AreEqual(5, tokens.Count);
            Assert.IsInstanceOfType(tokens[0], typeof(TypeToken));
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
