using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler.Entities;
using System.Collections.Generic;
using Assembler.Entities.Tokens;

namespace Assembler.Test
{
    [TestClass]
    public class ConvertToTokens_Tests
    {
        [TestMethod]
        public void ConvertToTokens_0_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_00"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(3, asmSource.Lines.Count);

            Assert.AreEqual("LD A, 5", asmSource.Lines[0].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[0].Tokens.Count);
            Assert.AreEqual("LD", asmSource.Lines[0].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("A", asmSource.Lines[0].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[1], typeof(RegisterToken));
            Assert.AreEqual(",", asmSource.Lines[0].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("5", asmSource.Lines[0].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[3], typeof(LiteralToken));
            Assert.AreEqual(5, ((LiteralToken)asmSource.Lines[0].Tokens[3]).NumericValue);

            Assert.AreEqual("label_01:", asmSource.Lines[1].Text.Trim());
            Assert.AreEqual(1, asmSource.Lines[1].Tokens.Count);
            Assert.AreEqual("label_01", asmSource.Lines[1].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[0], typeof(LabelToken));

            Assert.AreEqual("INC A", asmSource.Lines[2].Text.Trim());
            //Assert.AreEqual(6, asmSource.Lines[2].Address);
            Assert.AreEqual(2, asmSource.Lines[2].Tokens.Count);
            Assert.AreEqual("INC", asmSource.Lines[2].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("A", asmSource.Lines[2].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[1], typeof(RegisterToken));
        }

        [TestMethod]
        public void ConvertToTokens_1_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_01"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(3, asmSource.Lines.Count);

            Assert.AreEqual("LD A, 0x5	// hexadecimal literal", asmSource.Lines[0].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[0].Tokens.Count);
            Assert.AreEqual("LD", asmSource.Lines[0].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("A", asmSource.Lines[0].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[1], typeof(RegisterToken));
            Assert.AreEqual(",", asmSource.Lines[0].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("0x5", asmSource.Lines[0].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[3], typeof(LiteralToken));
            Assert.AreEqual(5, ((LiteralToken)asmSource.Lines[0].Tokens[3]).NumericValue);

            Assert.AreEqual("label_01:", asmSource.Lines[1].Text.Trim());
            Assert.AreEqual(1, asmSource.Lines[1].Tokens.Count);
            Assert.AreEqual("label_01", asmSource.Lines[1].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[0], typeof(LabelToken));

            Assert.AreEqual("INC A", asmSource.Lines[2].Text.Trim());
            Assert.AreEqual(2, asmSource.Lines[2].Tokens.Count);
            Assert.AreEqual("INC", asmSource.Lines[2].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("A", asmSource.Lines[2].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[1], typeof(RegisterToken));
        }

        [TestMethod]
        public void ConvertToTokens_1a_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_01a"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(3, asmSource.Lines.Count);

            Assert.AreEqual("LD A, 0b00000101	// binary literal", asmSource.Lines[0].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[0].Tokens.Count);
            Assert.AreEqual("LD", asmSource.Lines[0].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("A", asmSource.Lines[0].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[1], typeof(RegisterToken));
            Assert.AreEqual(",", asmSource.Lines[0].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("0b00000101", asmSource.Lines[0].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[3], typeof(LiteralToken));
            Assert.AreEqual(5, ((LiteralToken)asmSource.Lines[0].Tokens[3]).NumericValue);

            Assert.AreEqual("label_01:", asmSource.Lines[1].Text.Trim());
            Assert.AreEqual(1, asmSource.Lines[1].Tokens.Count);
            Assert.AreEqual("label_01", asmSource.Lines[1].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[0], typeof(LabelToken));

            Assert.AreEqual("INC A", asmSource.Lines[2].Text.Trim());
            Assert.AreEqual(2, asmSource.Lines[2].Tokens.Count);
            Assert.AreEqual("INC", asmSource.Lines[2].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("A", asmSource.Lines[2].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[1], typeof(RegisterToken));
        }

        [TestMethod]
        public void ConvertToTokens_1b_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_01b"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(3, asmSource.Lines.Count);

            Assert.AreEqual("LD A, 'a'	// ASCII character", asmSource.Lines[0].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[0].Tokens.Count);
            Assert.AreEqual("LD", asmSource.Lines[0].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("A", asmSource.Lines[0].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[1], typeof(RegisterToken));
            Assert.AreEqual(",", asmSource.Lines[0].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("'a'", asmSource.Lines[0].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[3], typeof(AscCharToken));
            Assert.AreEqual(97, ((AscCharToken)asmSource.Lines[0].Tokens[3]).NumericValue);

            Assert.AreEqual("label_01:", asmSource.Lines[1].Text.Trim());
            Assert.AreEqual(1, asmSource.Lines[1].Tokens.Count);
            Assert.AreEqual("label_01", asmSource.Lines[1].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[0], typeof(LabelToken));

            Assert.AreEqual("INC A", asmSource.Lines[2].Text.Trim());
            Assert.AreEqual(2, asmSource.Lines[2].Tokens.Count);
            Assert.AreEqual("INC", asmSource.Lines[2].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("A", asmSource.Lines[2].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[1], typeof(RegisterToken));
        }

        [TestMethod]
        public void ConvertToTokens_2_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_02"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(9, asmSource.Lines.Count);

            Assert.AreEqual("LD A, 0x0", asmSource.Lines[0].Text.Trim());
            //Assert.AreEqual(4, asmSource.Lines[0].Tokens.Count);
            //Assert.AreEqual("LD", asmSource.Lines[0].Tokens[0].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[0], typeof(CommandToken));
            //Assert.AreEqual("A", asmSource.Lines[0].Tokens[1].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[1], typeof(RegisterToken));
            //Assert.AreEqual(",", asmSource.Lines[0].Tokens[2].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[2], typeof(CommaToken));
            //Assert.AreEqual("0x5", asmSource.Lines[0].Tokens[3].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[3], typeof(LiteralToken));

            Assert.AreEqual("LD B, 0xFF", asmSource.Lines[1].Text.Trim());
            Assert.AreEqual("loop:", asmSource.Lines[2].Text.Trim());
            Assert.AreEqual("DEC B", asmSource.Lines[3].Text.Trim());
            Assert.AreEqual("JP Z, :end_loop", asmSource.Lines[4].Text.Trim());
            Assert.AreEqual("JP :loop", asmSource.Lines[5].Text.Trim());
            Assert.AreEqual("end_loop:", asmSource.Lines[6].Text.Trim());
            Assert.AreEqual("INC A", asmSource.Lines[7].Text.Trim());
            Assert.AreEqual("jp :loop", asmSource.Lines[8].Text.Trim());
        }

        [TestMethod]
        public void ConvertToTokens_3_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_03"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(9, asmSource.Lines.Count);

            Assert.AreEqual("LD A, 0x0", asmSource.Lines[0].Text.Trim());
            //Assert.AreEqual(4, asmSource.Lines[0].Tokens.Count);
            //Assert.AreEqual("LD", asmSource.Lines[0].Tokens[0].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[0], typeof(CommandToken));
            //Assert.AreEqual("A", asmSource.Lines[0].Tokens[1].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[1], typeof(RegisterToken));
            //Assert.AreEqual(",", asmSource.Lines[0].Tokens[2].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[2], typeof(CommaToken));
            //Assert.AreEqual("0x5", asmSource.Lines[0].Tokens[3].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[3], typeof(LiteralToken));

            Assert.AreEqual("LD B, 0xff", asmSource.Lines[1].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[1].Tokens.Count);
            Assert.AreEqual("LD", asmSource.Lines[1].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("B", asmSource.Lines[1].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[1], typeof(RegisterToken));
            Assert.AreEqual(",", asmSource.Lines[1].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("0xff", asmSource.Lines[1].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[3], typeof(LiteralToken));

            Assert.AreEqual("loop:", asmSource.Lines[2].Text.Trim());
            Assert.AreEqual("DEC B", asmSource.Lines[3].Text.Trim());
            Assert.AreEqual("JP Z, :end_loop", asmSource.Lines[4].Text.Trim());
            Assert.AreEqual("JP :loop", asmSource.Lines[5].Text.Trim());
            Assert.AreEqual("end_loop:", asmSource.Lines[6].Text.Trim());
            Assert.AreEqual("INC A", asmSource.Lines[7].Text.Trim());
            Assert.AreEqual("jp :loop", asmSource.Lines[8].Text.Trim());
        }

        [TestMethod]
        public void ConvertToTokens_4_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_04"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(15, asmSource.Lines.Count);

            Assert.AreEqual("LD A, 0x0", asmSource.Lines[0].Text.Trim());
            //Assert.AreEqual(4, asmSource.Lines[0].Tokens.Count);
            //Assert.AreEqual("LD", asmSource.Lines[0].Tokens[0].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[0], typeof(CommandToken));
            //Assert.AreEqual("A", asmSource.Lines[0].Tokens[1].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[1], typeof(RegisterToken));
            //Assert.AreEqual(",", asmSource.Lines[0].Tokens[2].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[2], typeof(CommaToken));
            //Assert.AreEqual("0x5", asmSource.Lines[0].Tokens[3].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[3], typeof(LiteralToken));

            Assert.AreEqual("LD B, 0xFF", asmSource.Lines[1].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[1].Tokens.Count);
            Assert.AreEqual("LD", asmSource.Lines[1].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("B", asmSource.Lines[1].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[1], typeof(RegisterToken));
            Assert.AreEqual(",", asmSource.Lines[1].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("0xFF", asmSource.Lines[1].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[1].Tokens[3], typeof(LiteralToken));

            Assert.AreEqual("ST [0xc00], A", asmSource.Lines[2].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[2].Tokens.Count);
            Assert.AreEqual("ST", asmSource.Lines[2].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("0xc00", asmSource.Lines[2].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[1], typeof(AddressToken));
            Assert.AreEqual(",", asmSource.Lines[2].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("A", asmSource.Lines[2].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[2].Tokens[3], typeof(RegisterToken));


            Assert.AreEqual("ST [0xc01], B", asmSource.Lines[3].Text.Trim());
            Assert.AreEqual("loop:", asmSource.Lines[4].Text.Trim());
            Assert.AreEqual("LD B, [0xc01]", asmSource.Lines[5].Text.Trim());
            Assert.AreEqual("DEC B", asmSource.Lines[6].Text.Trim());
            Assert.AreEqual("ST [0xc01], B", asmSource.Lines[7].Text.Trim());
            Assert.AreEqual("JP Z, :end_loop", asmSource.Lines[8].Text.Trim());
            Assert.AreEqual("JP :loop", asmSource.Lines[9].Text.Trim());
            Assert.AreEqual("end_loop:", asmSource.Lines[10].Text.Trim());
            Assert.AreEqual("LD A, [0xc00]", asmSource.Lines[11].Text.Trim());
            Assert.AreEqual("INC A", asmSource.Lines[12].Text.Trim());
            Assert.AreEqual("ST [0xc00], A", asmSource.Lines[13].Text.Trim());
            Assert.AreEqual("jp :loop", asmSource.Lines[14].Text.Trim());
        }

        [TestMethod]
        public void ConvertToTokens_5_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_05"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(17, asmSource.Lines.Count);

            Assert.AreEqual("#defbyte	var_A", asmSource.Lines[0].Text.Trim());
            Assert.AreEqual("#defbyte	var_B", asmSource.Lines[1].Text.Trim());

            Assert.AreEqual("LD A, 0x0", asmSource.Lines[2].Text.Trim());
            //Assert.AreEqual(4, asmSource.Lines[0].Tokens.Count);
            //Assert.AreEqual("LD", asmSource.Lines[0].Tokens[0].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[0], typeof(CommandToken));
            //Assert.AreEqual("A", asmSource.Lines[0].Tokens[1].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[1], typeof(RegisterToken));
            //Assert.AreEqual(",", asmSource.Lines[0].Tokens[2].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[2], typeof(CommaToken));
            //Assert.AreEqual("0x5", asmSource.Lines[0].Tokens[3].Text);
            //Assert.IsInstanceOfType(asmSource.Lines[0].Tokens[3], typeof(LiteralToken));

            Assert.AreEqual("LD B, 0xFF", asmSource.Lines[3].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[3].Tokens.Count);
            Assert.AreEqual("LD", asmSource.Lines[3].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[3].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("B", asmSource.Lines[3].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[3].Tokens[1], typeof(RegisterToken));
            Assert.AreEqual(",", asmSource.Lines[3].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[3].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("0xFF", asmSource.Lines[3].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[3].Tokens[3], typeof(LiteralToken));

            Assert.AreEqual("ST #var_A, A", asmSource.Lines[4].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[4].Tokens.Count);
            Assert.AreEqual("ST", asmSource.Lines[4].Tokens[0].Text);
            Assert.IsInstanceOfType(asmSource.Lines[4].Tokens[0], typeof(CommandToken));
            Assert.AreEqual("var_A", asmSource.Lines[4].Tokens[1].Text);
            Assert.IsInstanceOfType(asmSource.Lines[4].Tokens[1], typeof(DirectiveToken));
            Assert.AreEqual(",", asmSource.Lines[4].Tokens[2].Text);
            Assert.IsInstanceOfType(asmSource.Lines[4].Tokens[2], typeof(CommaToken));
            Assert.AreEqual("A", asmSource.Lines[4].Tokens[3].Text);
            Assert.IsInstanceOfType(asmSource.Lines[4].Tokens[3], typeof(RegisterToken));

            Assert.AreEqual("ST #var_B, B", asmSource.Lines[5].Text.Trim());
            Assert.AreEqual("loop:", asmSource.Lines[6].Text.Trim());

            Assert.AreEqual("LD B, #var_B", asmSource.Lines[7].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[7].Tokens.Count);

            Assert.AreEqual("DEC B", asmSource.Lines[8].Text.Trim());
            Assert.AreEqual("ST #var_B, B", asmSource.Lines[9].Text.Trim());
            Assert.AreEqual("JP Z, :end_loop", asmSource.Lines[10].Text.Trim());
            Assert.AreEqual("JP :loop", asmSource.Lines[11].Text.Trim());
            Assert.AreEqual("end_loop:", asmSource.Lines[12].Text.Trim());
            Assert.AreEqual("LD A, #var_A", asmSource.Lines[13].Text.Trim());
            Assert.AreEqual("INC A", asmSource.Lines[14].Text.Trim());
            Assert.AreEqual("ST #var_A, A", asmSource.Lines[15].Text.Trim());
            Assert.AreEqual("jp :loop", asmSource.Lines[16].Text.Trim());
        }

        [TestMethod]
        public void ConvertToTokens_6_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_06"));

            // Act
            AssemblerClass.ConvertToTokens(asmSource);

            // Assert
            Assert.AreEqual(7, asmSource.Lines.Count);

            Assert.AreEqual("delay:", asmSource.Lines[0].Text.Trim());
            Assert.AreEqual("DNW A", asmSource.Lines[1].Text.Trim());

            Assert.AreEqual("JP Z, :delay_end", asmSource.Lines[2].Text.Trim());
            Assert.AreEqual(4, asmSource.Lines[2].Tokens.Count);

            Assert.AreEqual("DEC A", asmSource.Lines[3].Text.Trim());

            Assert.AreEqual("JP :delay", asmSource.Lines[4].Text.Trim());
            Assert.AreEqual(2, asmSource.Lines[4].Tokens.Count);

            Assert.AreEqual("delay_end:", asmSource.Lines[5].Text.Trim());
            Assert.AreEqual("RET", asmSource.Lines[6].Text.Trim());
        }

    }
}
