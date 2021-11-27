using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RouteSheet.Data.Repositories;
using RouteSheet.Server.Controllers;
using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RouteSheet.Server.Tests
{
    public class CadetsControllerTest
    {
        [Fact(DisplayName = "Getting 5 cadets should return Ok, be typeof<List> and count 5")]
        public void Test_001()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.AllCadets())
                .Returns(GetTestCadets());

            var controller = new CadetsController(mockRepo.Object);

            // Act
            var cut = controller.GetCadets();
            var result = cut.Result as OkObjectResult;
            var value = result.Value as IList<Cadet>;

            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
            value.Should().BeOfType<List<Cadet>>();
            value.Count().Should().Be(5);
        }

        [Fact(DisplayName = "Getting empty list of cadets should return Ok, be typeof<List> and count 0")]
        public void Test_002()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.AllCadets())
                .Returns(new List<Cadet>().AsQueryable());

            var controller = new CadetsController(mockRepo.Object);

            // Act
            var cut = controller.GetCadets();
            var result = cut.Result as OkObjectResult;
            var value = result.Value as IList<Cadet>;

            result.StatusCode.Should().Be(200);
            value.Should().BeOfType<List<Cadet>>();
            value.Count().Should().Be(0);
        }

        [Fact(DisplayName = "Adding new cadet should return created cadet")]
        public async Task Test_003()
        {
            // Arrange
            var testCadet = TestCadet();
            var newCadet = new Cadet()
            {
                Name = testCadet.Name,
                Classroom = testCadet.Classroom,
                Lessons = testCadet.Lessons
            };
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.AddCadet(newCadet))
                .ReturnsAsync(testCadet);

            var controller = new CadetsController(mockRepo.Object);

            // Act
            var cut = await controller.Add(newCadet);
            var result = cut.Result as OkObjectResult;
            var value = result.Value as Cadet;

            result.StatusCode.Should().Be(200);
            value.Should().BeOfType<Cadet>();
            value.Id.Should().Be(testCadet.Id);
        }

        [Fact(DisplayName = "Updating existing cadet should return cadet with updated fields")]
        public async Task Test_004()
        {
            // Arrange
            var cadetInDb = TestCadet();
            var updatedCadet = new Cadet()
            {
                Id = cadetInDb.Id,
                Name = "Иванов И.И.",
                Classroom = cadetInDb.Classroom,
                Lessons = cadetInDb.Lessons
            };

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.FindCadetById(cadetInDb.Id))
                .ReturnsAsync(cadetInDb);
            mockRepo.Setup(repo => repo.UpdateCadet(updatedCadet))
                .ReturnsAsync(updatedCadet);

            var controller = new CadetsController(mockRepo.Object);

            // Act
            var cut = await controller.Update(updatedCadet);
            var result = cut.Result as OkObjectResult;
            var value = result.Value as Cadet;

            result.StatusCode.Should().Be(200);
            value.Should().BeOfType<Cadet>();
            value.Name.Should().Be("Иванов И.И.");
        }

        [Fact(DisplayName = "Updating not existing cadet should return Problem")]
        public async Task Test_005()
        {
            // Arrange
            var testCadet = TestCadet();

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.FindCadetById(testCadet.Id))
                .ReturnsAsync((Cadet)null);

            var controller = new CadetsController(mockRepo.Object);

            // Act
            var cut = await controller.Update(testCadet);
            var result = cut.Result as ObjectResult;
            var value = result.Value as ProblemDetails;

            result.StatusCode.Should().Be(500);
            value.Title.Should().Be("Api layer problems, see details for more info");
        }

        [Fact(DisplayName = "Delete existing cadet should return NoContent")]
        public async Task Test_006()
        {
            // Arrange
            var cadetInDb = TestCadet();

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.DeleteCadet(cadetInDb.Id))
                .ReturnsAsync(true);

            var controller = new CadetsController(mockRepo.Object);

            // Act
            var cut = await controller.Delete(cadetInDb.Id);
            var result = cut as NoContentResult;

            result.StatusCode.Should().Be(204);
        }

        [Fact(DisplayName = "Delete not existing cadet should return BadRequest")]
        public async Task Test_007()
        {
            // Arrange
            var cadetInDb = TestCadet();

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.DeleteCadet(cadetInDb.Id))
                .ReturnsAsync(false);

            var controller = new CadetsController(mockRepo.Object);

            // Act
            var cut = await controller.Delete(cadetInDb.Id);
            var result = cut as BadRequestResult;

            result.StatusCode.Should().Be(400);
        }

        private Cadet TestCadet()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture.Create<Cadet>();
        }

        private IQueryable<Cadet> GetTestCadets()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture.CreateMany<Cadet>(5).AsQueryable();
        }
    }
}
