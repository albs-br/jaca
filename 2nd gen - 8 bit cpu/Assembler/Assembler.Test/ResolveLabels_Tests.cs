using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Assembler.Entities;

namespace Assembler.Test
{
    [TestClass]
    public class ResolveLabels_Tests
    {

        [TestMethod]
        public void ResolveLabels_1_Test()
        {
            // Arrange
            var asmSource = Utilities.GetFromFile("Test_01");

            // Act
            var machineCodeProgram = Converter.ResolveLabels(asmSource);

            // Assert
            Assert.AreEqual(1, machineCodeProgram.Labels.Count);
            Assert.AreEqual(3, machineCodeProgram.Labels["label_01"]);
        }

        [TestMethod]
        public void ResolveLabels_2_Test()
        {
            // Arrange
            var asmSource = Utilities.GetFromFile("Test_02");

            // Act
            var machineCodeProgram = Converter.ResolveLabels(asmSource);

            // Assert
            Assert.AreEqual(2, machineCodeProgram.Labels.Count);
            Assert.AreEqual(0x0006, machineCodeProgram.Labels["loop"]);
            Assert.AreEqual(0x000f, machineCodeProgram.Labels["end_loop"]);
        }
    }
}
