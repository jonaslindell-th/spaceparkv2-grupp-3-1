using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RestAPI.Controllers;
using RestAPI.Data;
using RestAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Moq.EntityFrameworkCore;
using RestAPI.ParkingLogic;
using RestAPI.Requests;
using Xunit;

namespace RestApiTests
{
    public class UnitTest1
    {
        private ISpacePort _spacePort = new SpacePort();
        private IParking _parking = new Parking();
        private IDbQueries _dbQueries = new DbQueries();
        private IReceipt _receipt = new Receipt();
        private ICalculate _calculate = new Calculate();

        [Theory]
        [InlineData("Luke Skywalker")]
        [InlineData("Darth Vader")]
        [InlineData("Yoda")]
        [InlineData("LUKE SKYWALKER")]
        [InlineData("yoda")]
        [InlineData("DaRtH vAdER")]
        public void Park_Valid_Starwars_Character_And_Ship_Expect_Parked(string name)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = "X-Wing";

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };

            for (int i = 0; i < 5; i++)
            {
                Parking parking = new Parking()
                {
                    Id = i,
                    Size = size,
                    SizeId = 1,
                    SpacePortId = 1,
                    CharacterName = null,
                    SpaceshipName = null,
                    Arrival = null
                };
                parkings.Add(parking);
            }
            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);


            //Act
            userController.Park(spacePorts[0].Id, request);
            var foundParking = spacePorts[0].Parkings.FirstOrDefault(p => p.CharacterName == request.PersonName);

            //Assert
            Assert.Equal(foundParking.CharacterName, request.PersonName);
        }

        [Theory]
        [InlineData("Luke Skywalker", "Toyota")]
        [InlineData("Darth Vader", "Something-Wing")]
        [InlineData("Yoda", "X-Wings")]
        public void Park_Valid_Starwars_Character_And_Invalid_Ship_Expect_Not_Parked(string name, string shipName)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = shipName;

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };

            for (int i = 0; i < 5; i++)
            {
                Parking parking = new Parking()
                {
                    Id = i,
                    Size = size,
                    SizeId = 1,
                    SpacePortId = 1,
                    CharacterName = null,
                    SpaceshipName = null,
                    Arrival = null
                };
                parkings.Add(parking);
            }
            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);


            //Act
            userController.Park(spacePorts[0].Id, request);
            var foundParking = spacePorts[0].Parkings.FirstOrDefault(p => p.CharacterName == request.PersonName);

            //Assert
            Assert.Null(foundParking);
        }

        [Theory]
        [InlineData("Brad Pitt")]
        [InlineData("Spuute")]
        [InlineData("Luke Skyscraper")]
        [InlineData("Darth vaden")]
        public void Park_Invalid_Starwars_Character_And_Valid_Ship_Expect_Not_Parked(string name)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = "X-Wing";

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };

            for (int i = 0; i < 5; i++)
            {
                Parking parking = new Parking()
                {
                    Id = i,
                    Size = size,
                    SizeId = 1,
                    SpacePortId = 1,
                    CharacterName = null,
                    SpaceshipName = null,
                    Arrival = null
                };
                parkings.Add(parking);
            }
            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);


            //Act
            userController.Park(spacePorts[0].Id, request);
            var foundParking = spacePorts[0].Parkings.FirstOrDefault(p => p.CharacterName == request.PersonName);

            //Assert
            Assert.Null(foundParking);
        }

        [Theory]
        [InlineData("Luke Skywalker")]
        public void Park_Valid_Starwars_Character_And_Ship_Then_Get_Active_Parkings_Expect_Found(string name)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = "X-Wing";

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };

            for (int i = 0; i < 5; i++)
            {
                Parking parking = new Parking()
                {
                    Id = i,
                    Size = size,
                    SizeId = 1,
                    SpacePortId = 1,
                    CharacterName = null,
                    SpaceshipName = null,
                    Arrival = null
                };
                parkings.Add(parking);
            }
            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);


            //Act
            userController.Park(spacePorts[0].Id, request);
            //var response = userController.ActiveParkings()

            //Assert
            //Assert.Equal(foundParking.CharacterName, request.PersonName);
        }
    }
}
