using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assembler.Entities;
using System.Collections.Generic;

namespace Assembler.Test
{
    [TestClass]
    public class ConvertTokensToMachineCode_Tests
    {
        private AsmSource ArrangeAndAct(string filename)
        {
            var source = Utilities.GetFromFile(filename);

            var asmSource = new AsmSource(source);
            AssemblerClass.ResolveIncludes(asmSource);
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
            var asmSource = ArrangeAndAct("Test_01");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(3, asmSource.Labels["label_01"]);
        }

        [TestMethod]
        public void ConvertTokensToMachineCode_1a_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_01a");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(3, asmSource.Labels["label_01"]);
        }

        [TestMethod]
        public void ConvertTokensToMachineCode_1b_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_01b");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(3, asmSource.Labels["label_01"]);
        }

        [TestMethod]
        public void ConvertSource_2_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_02");

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
            var asmSource = ArrangeAndAct("Test_03");

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
            var asmSource = ArrangeAndAct("Test_04");

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
            var asmSource = ArrangeAndAct("Test_05");

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
            var asmSource = ArrangeAndAct("Test_06");

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

        [TestMethod]
        public void ConvertSource_7_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_07");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x00a, asmSource.Labels["print_number_1digit"]);


            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x07,
                0x1c, 0x00, 0x0a,
                0x14, 0x00, 0x00,

                0x00, 

                0x07, 0x80, 0x30,
                0x80, 0x70, 0x00,
                0x44, 0x00, 0x00,
                0x24, 0x00, 0x00,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_8_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_08");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x00a, asmSource.Labels["print_number_1digit"]);


            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x07,
                0x1c, 0x00, 0x0a,
                0x14, 0x00, 0x00,

                0x00,

                0x07, 0x80, 0x30,
                0x80, 0x70, 0x00,
                0x44, 0x00, 0x00,
                0x24, 0x00, 0x00,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_9_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_09");

            // Assert
            Assert.AreEqual(5, asmSource.Labels.Count);
            Assert.AreEqual(0x010, asmSource.Labels["print_number_1digit"]);
            Assert.AreEqual(30, asmSource.Labels["print_number_2digit"]);
            Assert.AreEqual(39, asmSource.Labels["pn_loop"]);
            Assert.AreEqual(60, asmSource.Labels["pn_end"]);
            Assert.AreEqual(78, asmSource.Labels["pn_last_dig"]);


            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0x04,
                0x1c, 0x00, 0x10,
                0x04, 0x00, 0x63,
                0x1c, 0x00, 0x1e,
                0x14, 0x00, 0x0c,
                
                0x00,

                0x07, 0x80, 0x30,
                0x80, 0x70, 0x00,
                0x44, 0x00, 0x00,
                0x24, 0x00, 0x00,

                0x00, 0x00,

                0x0b, 0x20, 0x00,
                0x06, 0x80, 0x0a,
                0x05, 0x00, 0x00,
                0x08, 0x80, 0x00,
                0x84, 0x50, 0x00,
                0x30, 0x00, 0x3c,
                0x84, 0xd0, 0x00,
                0x08, 0x10, 0x00,
                0xa1, 0x00, 0x00,
                0x14, 0x00, 0x27,
                0x07, 0x80, 0x30,
                0xa9, 0x00, 0x00,
                0x18, 0x00, 0x4e,
                0x81, 0x70, 0x00,
                0x08, 0x20, 0x00,
                0x44, 0x00, 0x00,
                0x80, 0xf0, 0x00,
                0x08, 0x10, 0x00,
                0x44, 0x00, 0x00,
                0x09, 0x60, 0x00,
                0x24, 0x00, 0x00,

            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_10_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_10");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x0, asmSource.Labels["start"]);
            Assert.AreEqual(1, asmSource.DefMems.Count);
            //Assert.AreEqual(1, asmSource.DefMems[0]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0xff,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00,
                97
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_10a_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_10a");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x0, asmSource.Labels["start"]);
            Assert.AreEqual(1, asmSource.DefMems.Count);
            //Assert.AreEqual(1, asmSource.DefMems[0]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0xff,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00,
                97
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_11_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_11");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x0, asmSource.Labels["start"]);
            Assert.AreEqual(2, asmSource.DefMems.Count);
            //Assert.AreEqual(1, asmSource.DefMems[0]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0xff,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00,
                97,
                0x00,
                0xf0
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_11a_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_11a");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x0, asmSource.Labels["start"]);
            Assert.AreEqual(2, asmSource.DefMems.Count);
            //Assert.AreEqual(1, asmSource.DefMems[0]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0xff,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00,
                97,
                0x00,
                0xf0
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_12_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_12");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x0, asmSource.Labels["start"]);
            Assert.AreEqual(4, asmSource.DefMems.Count);
            //Assert.AreEqual(1, asmSource.DefMems[0]);

            var expected = new List<byte>(new byte[] {
                0x04, 0x00, 0xff,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00,
                97,
                0xf0,
                0x01, //0b0000_0001, // binary literals only in C#7.0 (.net 4.6.2)
                0x00,
                0x20
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_13_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_13");

            // Assert
            Assert.AreEqual(1, asmSource.Labels.Count);
            Assert.AreEqual(0x3, asmSource.Labels["start"]);
            Assert.AreEqual(4, asmSource.DefMems.Count);
            //Assert.AreEqual(1, asmSource.DefMems[0]);

            var expected = new List<byte>(new byte[] {
                0x00, 0x00, 0x00,
                0x04, 0x00, 0xff,
                0x00, 0x00, 0x00,
                0x00,
                97,
                0xf0,
                0x01, //0b0000_0001,
                0x00,
                0x20
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_14_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_14");

            // Assert
            Assert.AreEqual(5, asmSource.Labels.Count);
            Assert.AreEqual(0x3, asmSource.Labels["start"]);
            Assert.AreEqual(30, asmSource.Labels["print_number_2digit"]);
            Assert.AreEqual(39, asmSource.Labels["pn_loop"]);
            Assert.AreEqual(60, asmSource.Labels["pn_end"]);
            Assert.AreEqual(78, asmSource.Labels["pn_last_dig"]);

            Assert.AreEqual(4, asmSource.DefMems.Count);
            //Assert.AreEqual(1, asmSource.DefMems[0]);

            var expected = new List<byte>(new byte[] {
                0x00, 0x00, 0x00,
                0x04, 0x00, 0xff,
                0x00, 0x00, 0x00,
                0x00,
                97,
                0xf0,
                0x01, //0b0000_0001,
                0x00,
                0x20,

                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,

                0x0b, 0x20, 0x00,
                0x06, 0x80, 0x0a,
                0x05, 0x00, 0x00,
                0x08, 0x80, 0x00,
                0x84, 0x50, 0x00,
                0x30, 0x00, 0x3c,
                0x84, 0xd0, 0x00,
                0x08, 0x10, 0x00,
                0xa1, 0x00, 0x00,
                0x14, 0x00, 0x27,
                0x07, 0x80, 0x30,
                0xa9, 0x00, 0x00,
                0x18, 0x00, 0x4e,
                0x81, 0x70, 0x00,
                0x08, 0x20, 0x00,
                0x44, 0x00, 0x00,
                0x80, 0xf0, 0x00,
                0x08, 0x10, 0x00,
                0x44, 0x00, 0x00,
                0x09, 0x60, 0x00,
                0x24, 0x00, 0x00,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertSource_15_Test()
        {
            // Arrange & Act
            var asmSource = ArrangeAndAct("Test_15");

            // Assert
            Assert.AreEqual(5, asmSource.Labels.Count);
            Assert.AreEqual(0x3, asmSource.Labels["start"]);
            Assert.AreEqual(30, asmSource.Labels["print_number_2digit"]);
            Assert.AreEqual(39, asmSource.Labels["pn_loop"]);
            Assert.AreEqual(60, asmSource.Labels["pn_end"]);
            Assert.AreEqual(78, asmSource.Labels["pn_last_dig"]);

            Assert.AreEqual(4, asmSource.DefMems.Count);
            //Assert.AreEqual(1, asmSource.DefMems[0]);

            var expected = new List<byte>(new byte[] {
                0x00, 0x00, 0x00,
                0x04, 0x00, 0xff,
                0x00, 0x00, 0x00,
                0x00,
                97,
                0xf0,
                0x01, //0b0000_0001,
                0x00,
                0x20,

                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,
                0x00, 0x00, 0x00,

                0x0b, 0x20, 0x00,
                0x06, 0x80, 0x0a,
                0x05, 0x00, 0x00,
                0x08, 0x80, 0x00,
                0x84, 0x50, 0x00,
                0x30, 0x00, 0x3c,
                0x84, 0xd0, 0x00,
                0x08, 0x10, 0x00,
                0xa1, 0x00, 0x00,
                0x14, 0x00, 0x27,
                0x07, 0x80, 0x30,
                0xa9, 0x00, 0x00,
                0x18, 0x00, 0x4e,
                0x81, 0x70, 0x00,
                0x08, 0x20, 0x00,
                0x44, 0x00, 0x00,
                0x80, 0xf0, 0x00,
                0x08, 0x10, 0x00,
                0x44, 0x00, 0x00,
                0x09, 0x60, 0x00,
                0x24, 0x00, 0x00,
            });
            var actual = ((List<byte>)asmSource.Bytes);
            CollectionAssert.AreEqual(expected, actual);
        }

    }
}
