using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler.Entities;
using System.Collections.Generic;

namespace Assembler.Test
{
    [TestClass]
    public class ConvertTokensToMachineCode_Tests
    {
        private AsmSource ArrangeAndAct(string source)
        {
            var asmSource = new AsmSource(source);
            AssemblerClass.ConvertToTokens(asmSource);
            AssemblerClass.ResolveLabelsAndDirectives(asmSource);
            AssemblerClass.ConvertTokensToMachineCode(asmSource);

            // Act
            return asmSource;
        }

        [TestMethod]
        public void ConvertTokensToMachineCode_1_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct(Utilities.GetFromFile("Test_01"));

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(3, asmSource.Labels["label_01"]);
        }

        [TestMethod]
        public void ConvertSource_2_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct(Utilities.GetFromFile("Test_02"));

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(0x0006, asmSource.Labels["loop"]);
            Assert.AreEqual(0x000f, asmSource.Labels["end_loop"]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x00,
                0x04, 0x80, 0xff,
                0xa4, 0x80, 0x00,
                0x18, 0x00, 0x0f,
                0x14, 0x00, 0x06,
                0xa0, 0x00, 0x00,
                0x14, 0x00, 0x06,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_3_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct(Utilities.GetFromFile("Test_03"));

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(0x0006, asmSource.Labels["loop"]);
            Assert.AreEqual(0x000f, asmSource.Labels["end_loop"]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x00,
                0x04, 0x80, 0xff,
                0xa4, 0x80, 0x00,
                0x18, 0x00, 0x0f,
                0x14, 0x00, 0x06,
                0xa0, 0x00, 0x00,
                0x14, 0x00, 0x06,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_4_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct(Utilities.GetFromFile("Test_04"));

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(0x000c, asmSource.Labels["loop"]);
            Assert.AreEqual(0x001b, asmSource.Labels["end_loop"]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x00,
                0x04, 0x80, 0xff,
                0x28, 0x0c, 0x00,
                0x28, 0x8c, 0x01,

                0x0c, 0x8c, 0x01,
                0xa4, 0x80, 0x00,
                0x28, 0x8c, 0x01,
                0x18, 0x00, 0x1b,
                0x14, 0x00, 0x0c,

                0x0c, 0x0c, 0x00,
                0xa0, 0x00, 0x00,
                0x28, 0x0c, 0x00,
                0x14, 0x00, 0x0c,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_5_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct(Utilities.GetFromFile("Test_05"));

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(0x000c, asmSource.Labels["loop"]);
            Assert.AreEqual(0x001b, asmSource.Labels["end_loop"]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x00,
                0x04, 0x80, 0xff,
                0x28, 0x0c, 0x00,
                0x28, 0x8c, 0x01,

                0x0c, 0x8c, 0x01,
                0xa4, 0x80, 0x00,
                0x28, 0x8c, 0x01,
                0x18, 0x00, 0x1b,
                0x14, 0x00, 0x0c,

                0x0c, 0x0c, 0x00,
                0xa0, 0x00, 0x00,
                0x28, 0x0c, 0x00,
                0x14, 0x00, 0x0c,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_6_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct(Utilities.GetFromFile("Test_06"));

            // Assert
            Assert.AreEqual(2, asmSource.Labels.Count);
            Assert.AreEqual(0x0000, asmSource.Labels["delay"]);
            Assert.AreEqual(0x000c, asmSource.Labels["delay_end"]);

            var expected = new List<byte>(new byte[] {
                0xa8, 0x00, 0x00,
                0x18, 0x00, 0x0c,
                0xa4, 0x00, 0x00,
                0x14, 0x00, 0x00,
                0x24, 0x00, 0x00,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
