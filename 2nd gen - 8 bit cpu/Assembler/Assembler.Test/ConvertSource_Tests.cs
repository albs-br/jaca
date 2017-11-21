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
    }
}
