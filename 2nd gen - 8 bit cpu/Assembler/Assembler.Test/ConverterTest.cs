using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Assembler.Test
{
    [TestClass]
    public class ConverterTest
    {
        Converter converter = new Converter();
        
        [TestMethod]
        public void ConvertLine_ImediateInstruction_Test()
        {
            // Arrange
            var line = "LD    A, 0x01";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(1, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ImediateInstruction1_Test()
        {
            // Arrange
            var line = "LD B, 0x0A";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(128, bytes[1]);
            Assert.AreEqual(10, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ImediateInstruction2_Test()
        {
            // Arrange
            var line = "LD C, 0xFF";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(5, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ByRegisterInstruction_Test()
        {
            // Arrange
            var line = "LD A, B";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(16, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

    }
}
