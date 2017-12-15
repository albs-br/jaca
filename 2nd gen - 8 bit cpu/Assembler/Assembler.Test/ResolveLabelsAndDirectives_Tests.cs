using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Assembler.Entities;

namespace Assembler.Test
{
    [TestClass]
    public class ResolveLabelsAndDirectives_Tests
    {
        [TestMethod]
        public void ResolveLabelsAndDirectives_0_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_00"));
            AssemblerClass.ConvertToTokens(asmSource);

            // Act
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(3, asmSource.Labels["label_01"]);

            Assert.AreEqual(0, asmSource.Variables.Count);
        }

        [TestMethod]
        public void ResolveLabelsAndDirectives_1_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_01"));
            AssemblerClass.ConvertToTokens(asmSource);

            // Act
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x0003, asmSource.Labels["label_01"]);

            Assert.AreEqual(0, asmSource.Variables.Count);
        }

        [TestMethod]
        public void ResolveLabelsAndDirectives_2_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_02"));
            AssemblerClass.ConvertToTokens(asmSource);

            // Act
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(0x0006, asmSource.Labels["loop"]);
            Assert.AreEqual(0x000f, asmSource.Labels["end_loop"]);

            Assert.AreEqual(0, asmSource.Variables.Count);
        }

        [TestMethod]
        public void ResolveLabelsAndDirectives_3_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_03"));
            AssemblerClass.ConvertToTokens(asmSource);

            // Act
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(0x0006, asmSource.Labels["loop"]);
            Assert.AreEqual(0x000f, asmSource.Labels["end_loop"]);

            Assert.AreEqual(0, asmSource.Variables.Count);
        }

        [TestMethod]
        public void ResolveLabelsAndDirectives_4_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_04"));
            AssemblerClass.ConvertToTokens(asmSource);

            // Act
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(12, asmSource.Labels["loop"]);
            Assert.AreEqual(27, asmSource.Labels["end_loop"]);

            Assert.AreEqual(0, asmSource.Variables.Count);
        }

        [TestMethod]
        public void ResolveLabelsAndDirectives_5_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_05"));
            AssemblerClass.ConvertToTokens(asmSource);

            // Act
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(12, asmSource.Labels["loop"]);
            Assert.AreEqual(27, asmSource.Labels["end_loop"]);

            Assert.AreEqual(2, asmSource.Variables.Count);
            Assert.AreEqual(0xc00, asmSource.Variables["var_A"]);
            Assert.AreEqual(0xc01, asmSource.Variables["var_B"]);
        }

        [TestMethod]
        public void ResolveLabelsAndDirectives_6_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_06"));
            AssemblerClass.ConvertToTokens(asmSource);

            // Act
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(0, asmSource.Labels["delay"]);
            Assert.AreEqual(12, asmSource.Labels["delay_end"]);

            Assert.AreEqual(0, asmSource.Variables.Count);
        }

        [TestMethod]
        public void ResolveLabelsAndDirectives_7_Test()
        {
            // Arrange
            var asmSource = new AsmSource(Utilities.GetFromFile("Test_07"));
            AssemblerClass.ConvertToTokens(asmSource);

            // Act
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x00a, asmSource.Labels["print_number_1digit"]);

            Assert.AreEqual(7, asmSource.Lines.Count);
            Assert.AreEqual(0x000, asmSource.Lines[0].Address);
            Assert.AreEqual(0x003, asmSource.Lines[1].Address);
            Assert.AreEqual(0x006, asmSource.Lines[2].Address);
            Assert.AreEqual(0x00a, asmSource.Lines[3].Address);
            Assert.AreEqual(0x00d, asmSource.Lines[4].Address);
            Assert.AreEqual(0x010, asmSource.Lines[5].Address);
            Assert.AreEqual(0x013, asmSource.Lines[6].Address);

            Assert.AreEqual(0, asmSource.Variables.Count);
        }

    }
}
