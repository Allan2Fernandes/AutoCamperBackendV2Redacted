using AutoCamperBackendV2.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using AutoCamperBackendV2.Services;
using AutoCamperBackendV2.DataTransferObjects;
using AutoCamperBackendV2.Functions;

namespace XUnitTestProject
{
    public class UserTests
    {
        [Fact]
        public void CreateUserTest()
        {
            var mockSet = new Mock<DbSet<TblUser>>();
            var mockContext = new Mock<ParkInPeaceProjectContext>();

            mockContext.Setup(m => m.TblUsers).Returns(mockSet.Object);

            var service = new UserServices(mockContext.Object);
            service.AddUserToDatabase(new TblUser { FldEmail = "user@mail.com", FldEncryptedPassword = "123" });

            mockSet.Verify(m => m.Add(It.IsAny<TblUser>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }

        [Fact]
        public void LoginTest()
        {
            var data = new List<TblUser>
            {
                new TblUser
                {
                    FldUserId = 1,
                    FldEmail = "User1@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("123456")
                },
                new TblUser
                {
                    FldUserId = 2,
                    FldEmail = "User2@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("1234567")
                },
                new TblUser
                {
                    FldUserId = 3,
                    FldEmail = "User3@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("12345678") 
                }             
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TblUser>>();
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ParkInPeaceProjectContext>();
            mockContext.Setup(c => c.TblUsers).Returns(mockSet.Object);

            var Service = new UserServices(mockContext.Object);
            var QueriedUser = Service.FindSingleUserOnEmailAndPassword(new UserLoginDTO { FldEmail = "User2@mail.com", FldEncryptedPassword = "1234567" });


            Assert.Equal("User2@mail.com", QueriedUser.FldEmail);
            Assert.Equal(2, QueriedUser.FldUserId);
        }

        [Fact]
        public void FindUserOnUserIDTest()
        {
            var data = new List<TblUser>
            {
                new TblUser
                {
                    FldUserId = 1,
                    FldEmail = "User1@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("123456")
                },
                new TblUser
                {
                    FldUserId = 2,
                    FldEmail = "User2@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("1234567")
                },
                new TblUser
                {
                    FldUserId = 3,
                    FldEmail = "User3@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("12345678")
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TblUser>>();
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ParkInPeaceProjectContext>();
            mockContext.Setup(c => c.TblUsers).Returns(mockSet.Object);

            var Service = new UserServices(mockContext.Object);
            var QueriedUser = Service.FindUserOnUserID(3);

            Assert.Equal("User3@mail.com", QueriedUser.FldEmail);
        }

        [Fact]
        public void UpdateUserDetailsExcludingPasswordOnUserIDTest()
        {
            var data = new List<TblUser>
            {
                new TblUser
                {
                    FldUserId = 1,
                    FldEmail = "User1@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("123456"),
                    FldAdress = "Address1",
                    FldIsAdmin = true,
                    FldName = "User1",
                    FldPhoneNumber = "1234567890"                    
                },
                new TblUser
                {
                    FldUserId = 2,
                    FldEmail = "User2@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("1234567"),
                    FldAdress = "Address2",
                    FldIsAdmin = false,
                    FldName = "User´2",
                    FldPhoneNumber = "123456789"
                },
                new TblUser
                {
                    FldUserId = 3,
                    FldEmail = "User3@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("12345678"),
                    FldAdress = "Address3",
                    FldIsAdmin = true,
                    FldName = "User13",
                    FldPhoneNumber = "12345678"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TblUser>>();

            mockSet.As<IQueryable<TblUser>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ParkInPeaceProjectContext>();
            mockContext.Setup(c => c.TblUsers).Returns(mockSet.Object);

            // Create an object of the updated details
            /*
                    Email,
                    name,
                    phone,
                    Address,
                    (Optional) password
                 */

            // Test the case when the password is also getting changed
            UpdateUserDetailsDTO NewUserDetails1 = new UpdateUserDetailsDTO
            {
                FldEmail = "Changed@mail.com",
                FldUserId = 3,
                FldName = "ChangedUser",
                FldPhoneNumber = "987",
                FldAddress = "ChangedAddress",
            };

            UserServices service = new UserServices(mockContext.Object);
            service.UpdateUserDetailsOnUserID(NewUserDetails1);
            // Verify that the changes have been saved
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            // Verify by re-quering the TblUser
            var QueriedUser = service.FindUserOnUserID(3);
            Assert.Equal("Changed@mail.com", QueriedUser.FldEmail);
            Assert.Equal("ChangedUser", QueriedUser.FldName);

            // Verify that the password was not changed
            Assert.Equal(Functions.EncryptPassword("12345678"), QueriedUser.FldEncryptedPassword);
        }

        [Fact]
        public void UpdateUserDetailsIncludingPasswordOnUserIDTest()
        {
            var data = new List<TblUser>
            {
                new TblUser
                {
                    FldUserId = 1,
                    FldEmail = "User1@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("123456"),
                    FldAdress = "Address1",
                    FldIsAdmin = true,
                    FldName = "User1",
                    FldPhoneNumber = "1234567890"
                },
                new TblUser
                {
                    FldUserId = 2,
                    FldEmail = "User2@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("1234567"),
                    FldAdress = "Address2",
                    FldIsAdmin = false,
                    FldName = "User´2",
                    FldPhoneNumber = "123456789"
                },
                new TblUser
                {
                    FldUserId = 3,
                    FldEmail = "User3@mail.com",
                    FldEncryptedPassword = Functions.EncryptPassword("12345678"),
                    FldAdress = "Address3",
                    FldIsAdmin = true,
                    FldName = "User13",
                    FldPhoneNumber = "12345678"
                }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<TblUser>>();

            mockSet.As<IQueryable<TblUser>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<TblUser>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            var mockContext = new Mock<ParkInPeaceProjectContext>();
            mockContext.Setup(c => c.TblUsers).Returns(mockSet.Object);

            // Create an object of the updated details
            /*
                    Email,
                    name,
                    phone,
                    Address,
                    (Optional) password
                 */

            // Test the case when the password is also getting changed
            UpdateUserDetailsDTO NewUserDetails1 = new UpdateUserDetailsDTO
            {
                FldEmail = "Changed@mail.com",
                FldUserId = 3,
                FldName = "ChangedUser",
                FldPhoneNumber = "987",
                FldAddress = "ChangedAddress",
                FldPassword = "ChangedPassword",
            };

            UserServices service = new UserServices(mockContext.Object);
            service.UpdateUserDetailsOnUserID(NewUserDetails1);
            // Verify that the changes have been saved
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            // Verify by re-quering the TblUser
            var QueriedUser = service.FindUserOnUserID(3);
            Assert.Equal("Changed@mail.com", QueriedUser.FldEmail);
            Assert.Equal("ChangedUser", QueriedUser.FldName);

            // Verify that the password was not changed
            Assert.Equal(Functions.EncryptPassword("ChangedPassword"), QueriedUser.FldEncryptedPassword);
        }
    }

   
}
