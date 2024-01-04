using AutoCamperBackendV2.Functions;
using AutoCamperBackendV2.Models;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;

namespace XUnitTestProject
{
    public class FunctionTests
    {

        // Case sensitive passwords
        [Fact]
        public void EncryptionTest1()
        {
            string stringToEncrypt = "Allan";
            string expectedEnryptedValue = "2823576859b5816a6e11186a25356117e0d67d0fffd233f1de55585806583d01";

            Assert.Equal(expectedEnryptedValue, Functions.EncryptPassword(stringToEncrypt));
        }


        [Fact]
        public void EncryptionTest2()
        {
            string stringToEncrypt = "allan";
            string expectedEnryptedValue = "5fd726bc56c3e86906cba8814a9b97d97f2ab209c48d2441ca5ac9e52b09e630";

            Assert.Equal(expectedEnryptedValue, Functions.EncryptPassword(stringToEncrypt));
        }
    }
}