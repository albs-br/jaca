using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Assembler.Entities;
using Assembler.Entities.Exceptions;

namespace Assembler.Test
{
    [TestClass]
    public class ConvertLineTokensToMachineCode_Tests
    {
        private byte[] ArrangeAndAct(string lineSrc)
        {
            var asmSource = new AsmSource(lineSrc);
            AssemblerClass.ConvertToTokens(asmSource);
            AssemblerClass.ConvertTokensToMachineCode(asmSource);
            var line = asmSource.Lines[0];

            // Act
            return AssemblerClass.ConvertLineTokensToMachineCode(line);
        }

        #region LD instructions

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_1_Test()
        {
            // Arrange
            var line = "LD    A, 0x01"; // Upper case statement

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(1, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_10_Test()
        {
            // Arrange
            var line = "LD    A, 1"; // Decimal literal

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(1, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_10a_Test()
        {
            // Arrange
            var line = "LD    A, 0b00000011"; // Binary literal

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(3, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_10ab_Test()
        {
            // Arrange
            var line = "LD    A, 0b11111111"; // Binary literal

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_10aba_Test()
        {
            // Arrange
            var line = "LD    A, 'a'"; // ASCII character

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(97, bytes[1]);
            Assert.AreEqual(255, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeLD_Imediate_10ac_Test()
        {
            // Arrange
            var line = "LD    A, 0b00012301"; // Binary literal with wrong digits

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeLD_Imediate_10b_Test()
        {
            // Arrange
            var line = "LD    A, 000abaab01"; // number with letters

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeLD_Imediate_10ba_Test()
        {
            // Arrange
            var line = "LD    A, 0x0gj5"; // hexadecimal number with wrong letters

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_1aa_Test()
        {
            // Arrange
            var line = "LD    A, 0x01   // Commentary";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(1, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeLD_Imediate_1ab_Test()
        {
            // Arrange
            var line = "LD    A, 0x01// Commentary";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_1a_Test()
        {
            // Arrange
            var line = "ld    a, 0x01"; // Lower case statement

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(1, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_1b_Test()
        {
            // Arrange
            var line = "\t\tLD    A, 0x01"; // Tabs in front

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(1, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_2_Test()
        {
            // Arrange
            var line = "LD B, 0x0a"; // address in lower case should work

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(4, bytes[0]);
            Assert.AreEqual(128, bytes[1]);
            Assert.AreEqual(10, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Imediate_3_Test()
        {
            // Arrange
            var line = "LD C, 0XFF"; // X in upper case should work

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x06, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeLD_Imediate_4_Test()
        {
            // Arrange
            var line = "LD AA, 0xFF";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_1_Test()
        {
            // Arrange
            var line = "LD A, B";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(16, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_1a_Test()
        {
            // Arrange
            var line = "LD A,B"; // Same as before, without space after comma

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(16, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_1b_Test()
        {
            // Arrange
            var line = "LD A, B" + Environment.NewLine;

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(16, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_1c_Test()
        {
            // Arrange
            var line = "LD A B"; // without comma

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_2_Test()
        {
            // Arrange
            var line = "LD B, A";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(8, bytes[0]);
            Assert.AreEqual(128, bytes[1]);
            Assert.AreEqual(0, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_3_Test()
        {
            // Arrange
            var line = "LD D, F";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x0a, bytes[0]);
            Assert.AreEqual(0xf0, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_4_Test()
        {
            // Arrange
            var line = "LD E, F";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x0b, bytes[0]);
            Assert.AreEqual(0x70, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_5_Test()
        {
            // Arrange
            var line = "LD A, H";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x08, bytes[0]);
            Assert.AreEqual(0x20, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeLD_ByRegister_6_Test()
        {
            // Arrange
            var line = "LD A, BB";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Direct_1_Test()
        {
            // Arrange
            var line = "LD A, [0x00C]"; // address with 12 bits (3 hexadecimal digits)

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(12, bytes[0]);
            Assert.AreEqual(0, bytes[1]);
            Assert.AreEqual(12, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Direct_2_Test()
        {
            // Arrange
            var line = "LD B, [0x8]";  // address with 4 bits (1 hexadecimal digit)

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(12, bytes[0]);
            Assert.AreEqual(128, bytes[1]);
            Assert.AreEqual(8, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Direct_3_Test()
        {
            // Arrange
            var line = "LD C, [0xFF]";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x0e, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Direct_4_Test()
        {
            // Arrange
            var line = "LD C, [0xFFF]";  // max addr possible with 12 bits

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x0e, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Direct_4a_Test()
        {
            // Arrange
            var line = "LD C, [4095]";  // max addr possible with 12 bits in decimal

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x0e, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Direct_4b_Test()
        {
            // Arrange
            var line = "LD C, [0b111111111111]";  // max addr possible with 12 bits in binary

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x0e, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_Direct_5_Test()
        {
            // Arrange
            var line = "LD C, [0xfff]";  // same as above with addr in lowercase

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x0e, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeLD_Direct_6_Test()
        {
            // Arrange
            var line = "LD AA, [0x00C]";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_IndirectByRegister_1_Test()
        {
            // Arrange
            var line = "LD A, [HL]";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x10, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeLD_IndirectByRegister_2_Test()
        {
            // Arrange
            var line = "LD B, [HL]";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x10, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion

        [TestMethod]
        public void ConvertLineTokensToMachineCodeCommentedLine_Test()
        {
            // Arrange
            var line = "// Commented line";

            // Act
            var asmSource = new AsmSource(line);
            AssemblerClass.ConvertToTokens(asmSource);
            AssemblerClass.ConvertTokensToMachineCode(asmSource);

            // Assert
            Assert.AreEqual(0, asmSource.Lines.Count);
        }

        #region ALU instructions

        [TestMethod]
        public void ConvertLineTokensToMachineCodeADD_ByRegister_1_Test()
        {
            // Arrange
            var line = "ADD A, E";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x80, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRegisterException))]
        public void ConvertLineTokensToMachineCodeADD_ByRegister_1a_Test()
        {
            // Arrange
            var line = "ADD A, B";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRegisterException))]
        public void ConvertLineTokensToMachineCodeADD_ByRegister_1b_Test()
        {
            // Arrange
            var line = "ADD C, D";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeADD_ByRegister_2_Test()
        {
            // Arrange
            var line = "ADD A";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeADD_ByRegister_3_Test()
        {
            // Arrange
            var line = "ADD AA, B";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeADD_ByRegister_4_Test()
        {
            // Arrange
            var line = "ADD A, BB";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeSUB_ByRegister_1_Test()
        {
            // Arrange
            var line = "SUB A, F";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x84, bytes[0]);
            Assert.AreEqual(0x70, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeNOT_ByRegister_1_Test()
        {
            // Arrange
            var line = "NOT A";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x88, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeAND_ByRegister_1_Test()
        {
            // Arrange
            var line = "AND B, E";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x8c, bytes[0]);
            Assert.AreEqual(0xe0, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeOR_ByRegister_1_Test()
        {
            // Arrange
            var line = "OR A, E";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x90, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeXOR_ByRegister_1_Test()
        {
            // Arrange
            var line = "XOR A, E";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x94, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeNOR_ByRegister_1_Test()
        {
            // Arrange
            var line = "NOR A, E";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x98, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeXNOR_ByRegister_1_Test()
        {
            // Arrange
            var line = "XNOR A, E";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x9c, bytes[0]);
            Assert.AreEqual(0x60, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeINC_ByRegister_1_Test()
        {
            // Arrange
            var line = "INC L";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0xa1, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRegisterException))]
        public void ConvertLineTokensToMachineCodeINC_ByRegister_1a_Test()
        {
            // Arrange
            var line = "INC C";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeINC_ByRegister_2_Test()
        {
            // Arrange
            var line = "INC";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCommandLineException))]
        public void ConvertLineTokensToMachineCodeINC_ByRegister_3_Test()
        {
            // Arrange
            var line = "INC AA";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeDEC_ByRegister_1_Test()
        {
            // Arrange
            var line = "DEC B";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0xa4, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeDNW_ByRegister_1_Test()
        {
            // Arrange
            var line = "DNW H";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0xa9, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeSHL_ByRegister_1_Test()
        {
            // Arrange
            var line = "SHL A";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0xb0, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeSHR_ByRegister_1_Test()
        {
            // Arrange
            var line = "SHR A";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0xb4, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeSUBM_ByRegister_1_Test()
        {
            // Arrange
            var line = "SUBM A, C";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0xac, bytes[0]);
            Assert.AreEqual(0x40, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion



        #region JP/CALL instructions

        [TestMethod]
        public void ConvertLineTokensToMachineCodeJP_Direct_1_Test()
        {
            // Arrange
            var line = "JP 0xFFF";  // max addr possible with 12 bits

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x14, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeJP_Z_Direct_1_Test()
        {
            // Arrange
            var line = "JP Z, 0xFFF";  // max addr possible with 12 bits

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x18, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeJP_C_Direct_1_Test()
        {
            // Arrange
            var line = "JP C, 0xFFF";  // max addr possible with 12 bits

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x30, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeCALL_Direct_1_Test()
        {
            // Arrange
            var line = "CALL 0xFFF";  // max addr possible with 12 bits

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x1c, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeCALL_Z_Direct_1_Test()
        {
            // Arrange
            var line = "CALL Z, 0xFFF";  // max addr possible with 12 bits

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x20, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeCALL_C_Direct_1_Test()
        {
            // Arrange
            var line = "CALL C, 0xFFF";  // max addr possible with 12 bits

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x34, bytes[0]);
            Assert.AreEqual(0x0f, bytes[1]);
            Assert.AreEqual(0xff, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeRET_Test()
        {
            // Arrange
            var line = "RET";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x24, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion



        #region ST instructions

        [TestMethod]
        public void ConvertLineTokensToMachineCodeST_Direct_1_Test()
        {
            // Arrange
            var line = "ST [0x00C], A"; // address with 12 bits (3 hexadecimal digits)

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x28, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x0c, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeST_Direct_2_Test()
        {
            // Arrange
            var line = "ST [0x00C], B"; // address with 12 bits (3 hexadecimal digits)

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x28, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x0c, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeST_IndirectByRegister_1_Test()
        {
            // Arrange
            var line = "ST [HL], A";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x2c, bytes[0]);
            Assert.AreEqual(0x00, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeST_IndirectByRegister_2_Test()
        {
            // Arrange
            var line = "ST [HL], B";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x2c, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        #endregion



        #region IN/OUT instructions

        [TestMethod]
        public void ConvertLineTokensToMachineCodeOUT_1_Test()
        {
            // Arrange
            var line = "OUT 0, B";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x44, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeOUT_2_Test()
        {
            // Arrange
            var line = "OUT 1, A, C";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x44, bytes[0]);
            Assert.AreEqual(0x42, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        public void ConvertLineTokensToMachineCodeIN_1_Test()
        {
            // Arrange
            var line = "IN 0, B";

            // Act
            var bytes = ArrangeAndAct(line);

            // Assert
            Assert.AreEqual(3, bytes.Length);
            Assert.AreEqual(0x40, bytes[0]);
            Assert.AreEqual(0x80, bytes[1]);
            Assert.AreEqual(0x00, bytes[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRegisterException))]
        public void ConvertLineTokensToMachineCodeIN_2_Test()
        {
            // Arrange
            var line = "IN 0, C";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRegisterException))]
        public void ConvertLineTokensToMachineCodeOUT_3_Test()
        {
            // Arrange
            var line = "OUT 0, E";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRegisterException))]
        public void ConvertLineTokensToMachineCodeOUT_4_Test()
        {
            // Arrange
            var line = "OUT 1, A, B";

            // Act
            var bytes = ArrangeAndAct(line);
        }

        #endregion
    }
}
