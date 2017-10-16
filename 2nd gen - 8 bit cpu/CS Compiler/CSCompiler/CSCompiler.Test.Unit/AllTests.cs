using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSCompiler.Entities;
using CSCompiler.Entities.CS;

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
            var machineCodeProgram = csProgram.Compile();

            // Assert
            Assert.AreEqual(32768, machineCodeProgram.bytes.Count);
            Assert.AreEqual(1, machineCodeProgram.bytes[2000]);
        }
    }
}
