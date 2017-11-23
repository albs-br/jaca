using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler.Entities;
using System.Collections.Generic;

namespace Assembler.Test
{
    [TestClass]
    public class ConvertSource_Tests
    {
        [TestMethod]
        public void ConvertSource_1_Test()
        {
            // Arrange
            var asmSource = Utilities.GetFromFile("Test_01");

            // Act
            var machineCodeProgram = Converter.ResolveLabels(asmSource);
            Converter.ConvertSource(machineCodeProgram);

            // Assert
            Assert.AreEqual(1, machineCodeProgram.Labels.Count);
            Assert.AreEqual(3, machineCodeProgram.Labels["label_01"]);
        }

        [TestMethod]
        public void ConvertSource_2_Test()
        {
            // Arrange
            var asmSource = Utilities.GetFromFile("Test_02");

            // Act
            var machineCodeProgram = Converter.ResolveLabels(asmSource);
            Converter.ConvertSource(machineCodeProgram);

            // Assert
            Assert.AreEqual(2, machineCodeProgram.Labels.Count);
            Assert.AreEqual(0x0006, machineCodeProgram.Labels["loop"]);
            Assert.AreEqual(0x000f, machineCodeProgram.Labels["end_loop"]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x00,
                0x04, 0x80, 0xff,
                0xa4, 0x80, 0x00,
                0x18, 0x00, 0x0f,
                0x14, 0x00, 0x06,
                0xa0, 0x00, 0x00,
                0x14, 0x00, 0x06,
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_3_Test()
        {
            // Arrange
            var asmSource = Utilities.GetFromFile("Test_03");

            // Act
            var machineCodeProgram = Converter.ResolveLabels(asmSource);
            Converter.ConvertSource(machineCodeProgram);

            // Assert
            Assert.AreEqual(2, machineCodeProgram.Labels.Count);
            Assert.AreEqual(0x0006, machineCodeProgram.Labels["loop"]);
            Assert.AreEqual(0x000f, machineCodeProgram.Labels["end_loop"]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x00,
                0x04, 0x80, 0xff,
                0xa4, 0x80, 0x00,
                0x18, 0x00, 0x0f,
                0x14, 0x00, 0x06,
                0xa0, 0x00, 0x00,
                0x14, 0x00, 0x06,
            });
            var actual = ((List<byte>)machineCodeProgram.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_4_Test()
        {
            // Arrange
            var asmSource = Utilities.GetFromFile("Test_04");

            // Act
            var machineCodeProgram = Converter.ResolveLabels(asmSource);
            Converter.ConvertSource(machineCodeProgram);

            // Assert
            Assert.AreEqual(2, machineCodeProgram.Labels.Count);
            Assert.AreEqual(0x000c, machineCodeProgram.Labels["loop"]);
            Assert.AreEqual(0x001b, machineCodeProgram.Labels["end_loop"]);

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
            var actual = ((List<byte>)machineCodeProgram.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_5_Test()
        {
            // Arrange
            var asmSource = Utilities.GetFromFile("Test_05");

            // Act
            var machineCodeProgram = Converter.ResolveLabels(asmSource);
            Converter.ConvertSource(machineCodeProgram);

            // Assert
            Assert.AreEqual(2, machineCodeProgram.Labels.Count);
            Assert.AreEqual(0x000c, machineCodeProgram.Labels["loop"]);
            Assert.AreEqual(0x001b, machineCodeProgram.Labels["end_loop"]);

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
            var actual = ((List<byte>)machineCodeProgram.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_6_Test()
        {
            // Arrange
            var asmSource = Utilities.GetFromFile("Test_06");

            // Act
            var machineCodeProgram = Converter.ResolveLabels(asmSource);
            Converter.ConvertSource(machineCodeProgram);

            // Assert
            Assert.AreEqual(2, machineCodeProgram.Labels.Count);
            Assert.AreEqual(0x0000, machineCodeProgram.Labels["delay"]);
            Assert.AreEqual(0x000c, machineCodeProgram.Labels["delay_end"]);

            //var expected = new List<byte>(new byte[] {
            //    0x04, 0x00, 0x00,
            //    0x04, 0x80, 0xff,
            //    0x28, 0x0c, 0x00,
            //    0x28, 0x8c, 0x01,

            //    0x0c, 0x8c, 0x01,
            //    0xa4, 0x80, 0x00,
            //    0x28, 0x8c, 0x01,
            //    0x18, 0x00, 0x1b,
            //    0x14, 0x00, 0x0c,

            //    0x0c, 0x0c, 0x00,
            //    0xa0, 0x00, 0x00,
            //    0x28, 0x0c, 0x00,
            //    0x14, 0x00, 0x0c,
            //});
            //var actual = ((List<byte>)machineCodeProgram.Bytes);
            //CollectionAssert.AreEqual(expected, actual);
        }
    }
}
