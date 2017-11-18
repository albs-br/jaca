using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler.Exceptions;

namespace Assembler.Test
{
    [TestClass]
    public class ConverterTest
    {
        Converter converter = new Converter();

        #region LD instructions

        [TestMethod]
        public void ConvertLine_LD_Imediate_1_Test()
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
        public void ConvertLine_LD_Imediate_1a_Test()
        {
            // Arrange
            var line = "\t\tLD    A, 0x01"; // Tabs in front

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(1, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_Imediate_2_Test()
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
        public void ConvertLine_LD_Imediate_3_Test()
        {
            // Arrange
            var line = "LD C, 0XFF"; // X in upper case should work

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x06, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_ByRegister_1_Test()
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
        public void ConvertLine_LD_ByRegister_1a_Test()
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
        public void ConvertLine_LD_ByRegister_1b_Test()
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
        public void ConvertLine_LD_ByRegister_2_Test()
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
        public void ConvertLine_LD_ByRegister_3_Test()
        {
            // Arrange
            var line = "LD D, F";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x0a, bytes[0]);
            Assert.AreEqual(0xf0, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_ByRegister_4_Test()
        {
            // Arrange
            var line = "LD E, F";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x0b, bytes[0]);
            Assert.AreEqual(0x70, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_ByRegister_5_Test()
        {
            // Arrange
            var line = "LD A, H";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x08, bytes[0]);
            Assert.AreEqual(0x20, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_Direct_1_Test()
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
        public void ConvertLine_LD_Direct_2_Test()
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
        public void ConvertLine_LD_Direct_3_Test()
        {
            // Arrange
            var line = "LD C, [0xFF]";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x0e, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_Direct_4_Test()
        {
            // Arrange
            var line = "LD C, [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x0e, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_Direct_5_Test()
        {
            // Arrange
            var line = "LD C, [0xfff]";  // same as above with addr in lowercase

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x0e, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_IndirectByRegister_1_Test()
        {
            // Arrange
            var line = "LD A, [HL]";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x10, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_LD_IndirectByRegister_2_Test()
        {
            // Arrange
            var line = "LD B, [HL]";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x10, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion

        [TestMethod]
        public void ConvertLine_CommentedLine_Test()
        {
            // Arrange
            var line = "// Commented line";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.IsNull(bytes);
        }

        #region ALU instructions

        [TestMethod]
        public void ConvertLine_ADD_ByRegister_1_Test()
        {
            // Arrange
            var line = "ADD A, E";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x80, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_SUB_ByRegister_1_Test()
        {
            // Arrange
            var line = "SUB A, F";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x84, bytes[0]);
            Assert.AreEqual(0x70, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_NOT_ByRegister_1_Test()
        {
            // Arrange
            var line = "NOT A";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x88, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_AND_ByRegister_1_Test()
        {
            // Arrange
            var line = "AND B, E";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x8c, bytes[0]);
            Assert.AreEqual(0xe0, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_OR_ByRegister_1_Test()
        {
            // Arrange
            var line = "OR A, E";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x90, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_XOR_ByRegister_1_Test()
        {
            // Arrange
            var line = "XOR A, E";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x94, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_NOR_ByRegister_1_Test()
        {
            // Arrange
            var line = "NOR A, E";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x98, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_XNOR_ByRegister_1_Test()
        {
            // Arrange
            var line = "XNOR A, E";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x9c, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_INC_ByRegister_1_Test()
        {
            // Arrange
            var line = "INC L";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0xa1, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_DEC_ByRegister_1_Test()
        {
            // Arrange
            var line = "DEC B";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0xa4, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_DNW_ByRegister_1_Test()
        {
            // Arrange
            var line = "DNW H";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0xa9, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_SUBM_ByRegister_1_Test()
        {
            // Arrange
            var line = "SUBM A, C";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0xac, bytes[0]);
            Assert.AreEqual(0x40, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion



        #region JP/CALL instructions

        [TestMethod]
        public void ConvertLine_JP_Direct_1_Test()
        {
            // Arrange
            var line = "JP [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x14, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_JP_Z_Direct_1_Test()
        {
            // Arrange
            var line = "JP Z, [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x18, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_JP_C_Direct_1_Test()
        {
            // Arrange
            var line = "JP C, [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x30, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_CALL_Direct_1_Test()
        {
            // Arrange
            var line = "CALL [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x1c, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_CALL_Z_Direct_1_Test()
        {
            // Arrange
            var line = "CALL Z, [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x20, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_CALL_C_Direct_1_Test()
        {
            // Arrange
            var line = "CALL C, [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x34, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_RET_Test()
        {
            // Arrange
            var line = "RET";  // max addr possible with 12 bits

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x24, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion



        #region ST instructions

        [TestMethod]
        public void ConvertLine_ST_Direct_1_Test()
        {
            // Arrange
            var line = "ST [0x00C], A"; // address with 12 bits (3 hexadecimal digits)

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x28, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x0c, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ST_Direct_2_Test()
        {
            // Arrange
            var line = "ST [0x00C], B"; // address with 12 bits (3 hexadecimal digits)

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x28, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x0c, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ST_IndirectByRegister_1_Test()
        {
            // Arrange
            var line = "ST [HL], A";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x2c, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_ST_IndirectByRegister_2_Test()
        {
            // Arrange
            var line = "ST [HL], B";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x2c, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion



        #region IN/OUT instructions

        [TestMethod]
        public void ConvertLine_OUT_1_Test()
        {
            // Arrange
            var line = "OUT 0, B";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x44, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLine_OUT_2_Test()
        {
            // Arrange
            var line = "OUT 1, A, C";

            // Act
            var bytes = converter.ConvertLine(line);

            // Assert
            Assert.AreEqual(0x44, bytes[0]);
            Assert.AreEqual(0x42, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion
    }
}
