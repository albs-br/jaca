using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler.Exceptions;

namespace Assembler.Test
{
    [TestClass]
    public class ConverterTest
    {
        Converter converter = new Converter();
        
        [TestMethod]
        public void ConvertLine_ImediateInstruction_1_Test()
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
        public void ConvertLine_ImediateInstruction_2_Test()
        {
            // Arrange
            var line = "LD B, 0x0a"; // address in lower case should work

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(128, bytes[1]);
            Assert.AreEqual(10, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ImediateInstruction_3_Test()
        {
            // Arrange
            var line = "LD C, 0XFF"; // X in upper case shoul work

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(5, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ByRegisterInstruction_1_Test()
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

        [TestMethod]
        public void ConvertLine_ByRegisterInstruction_1a_Test()
        {
            // Arrange
            var line = "LD A,B"; // Same as before, without space after comma

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(16, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ByRegisterInstruction_1b_Test()
        {
            // Arrange
            var line = "LD A, B"; // put crlf here

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(16, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ByRegisterInstruction_2_Test()
        {
            // Arrange
            var line = "LD B, A";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(128, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ByRegisterInstruction_3_Test()
        {
            // Arrange
            var line = "LD D, F";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(9, bytes[0]);
            Assert.AreEqual(208, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ByRegisterInstruction_4_Test()
        {
            // Arrange
            var line = "LD E, G";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(10, bytes[0]);
            Assert.AreEqual(96, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ByRegisterInstruction_5_Test()
        {
            // Arrange
            var line = "LD A, H";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(16 + 32 + 64, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_DirectInstruction_1_Test()
        {
            // Arrange
            var line = "LD A, [0x00C]"; // address with 12 bits (3 hexadecimal digits)

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(12, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(12, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_DirectInstruction_2_Test()
        {
            // Arrange
            var line = "LD B, [0x8]";  // address with 4 bits (1 hexadecimal digit)

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(12, bytes[0]);
            Assert.AreEqual(128, bytes[1]);
            Assert.AreEqual(8, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_DirectInstruction_3_Test()
        {
            // Arrange
            var line = "LD C, [0xFF]";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(13, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_DirectInstruction_4_Test()
        {
            // Arrange
            var line = "LD C, [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(13, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(4095, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(NotCommandLineException))]
        public void ConvertLine_CommentedLine_Test()
        {
            // Arrange
            var line = "// Commented line";

            // Act
            var bytes = converter.ConvertLine(line);
        }
    }
}
