using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RouteSheet.Data.Repositories;
using RouteSheet.Server.Controllers;
using RouteSheet.Shared.Models;
using RouteSheet.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RouteSheet.Server.Tests
{
    public class ClassroomsControllerTest
    {
        [Fact(DisplayName = "Getting 5 classrooms should return Ok, be typeof<List> and count 5")]
        public void Test_001()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.AllClassrooms())
                .Returns(GetTestClassrooms());
            var controller = new ClassroomsController(mockRepo.Object);

            // Act
            var cut = controller.GetClassrooms();
            var result = cut.Result as OkObjectResult;
            var value = result.Value as IList<Classroom>;

            result.StatusCode.Should().Be(200);
            result.Value.Should().NotBeNull();
            value.Should().BeOfType<List<Classroom>>();
            value.Count.Should().Be(5);
        }

        [Fact(DisplayName = "Getting empty list of classroom should return Ok, be typeof<List> and count 0")]
        public void Test_002()
        {
            // Arrange
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.AllClassrooms())
                .Returns(new List<Classroom>().AsQueryable());
            var controller = new ClassroomsController(mockRepo.Object);

            // Act
            var cut = controller.GetClassrooms();
            var result = cut.Result as OkObjectResult;
            var value = result.Value as IList<Classroom>;

            result.StatusCode.Should().Be(200);
            value.Should().BeOfType<List<Classroom>>();
            value.Count.Should().Be(0);
        }

        [Fact(DisplayName = "Adding new classroom should return created classroom")]
        public async Task Test_003()
        {
            // Arrange
            var testClassroom = TestClassroom();
            var newClassroom = new Classroom()
            {
                Cadets = testClassroom.Cadets,
                Name = testClassroom.Name
            };
            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.AddClassroom(newClassroom))
                .ReturnsAsync(testClassroom);
            var controller = new ClassroomsController(mockRepo.Object);

            // Act
            var newClassroomVM = newClassroom.ToClassroomViewModel();
            var cut = await controller.Add(newClassroomVM);
            var result = cut.Result as OkObjectResult;
            var value = result.Value as Classroom;

            result.StatusCode.Should().Be(200);
            value.Should().BeOfType<ClassroomViewModel>();
            value.Id.Should().Be(testClassroom.Id);
        }

        [Fact(DisplayName = "Updating existing classroom should return classroom with updated fields")]
        public async Task Test_004()
        {
            // Arrange
            var classroomInDb = TestClassroom();
            var updatedClassroom = new Classroom()
            {
                Id = classroomInDb.Id,
                Name = "9Г",
                Cadets= classroomInDb.Cadets
            };

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.FindClassroomById(classroomInDb.Id))
                .ReturnsAsync(classroomInDb);
            mockRepo.Setup(repo => repo.UpdateClassroom(updatedClassroom))
                .ReturnsAsync(updatedClassroom);
            var controller = new ClassroomsController(mockRepo.Object);

            // Act
            var updatedClassroomVM = updatedClassroom.ToClassroomViewModel();
            var cut = await controller.Update(updatedClassroomVM);
            var result = cut.Result as OkObjectResult;
            var value = result.Value as ClassroomViewModel;

            result.StatusCode.Should().Be(200);
            value.Should().BeOfType<ClassroomViewModel>();
            value.Name.Should().Be("9Г");
        }

        [Fact(DisplayName = "Updating not existing classroom should return Problem")]
        public async Task Test_005()
        {
            // Arrange
            var testClassroom = TestClassroom();

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.FindClassroomById(testClassroom.Id))
                .ReturnsAsync((Classroom)null);
            var controller = new ClassroomsController(mockRepo.Object);

            // Act
            var testClassroomVM = testClassroom.ToClassroomViewModel();
            var cut = await controller.Update(testClassroomVM);
            var result = cut.Result as ObjectResult;
            var value = result.Value as ProblemDetails;

            result.StatusCode.Should().Be(500);
            value.Title.Should().Be("Api layer problems, see details for more info");
        }

        [Fact(DisplayName = "Delete existing classroom should return NoContent")]
        public async Task Test_006()
        {
            // Arrange
            var classroomInDb = TestClassroom();

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.DeleteClassroom(classroomInDb.Id))
                .ReturnsAsync(true);
            var controller = new ClassroomsController(mockRepo.Object);

            // Act
            var cut = await controller.Delete(classroomInDb.Id);
            var result = cut as NoContentResult;

            result.StatusCode.Should().Be(204);
        }

        [Fact(DisplayName = "Delete not existing clasroom should return BadRequest")]
        public async Task Test_007()
        {
            // Arrange
            var classroomInDb = TestClassroom();

            var mockRepo = new Mock<IAppRepository>();
            mockRepo.Setup(repo => repo.DeleteClassroom(classroomInDb.Id))
                .ReturnsAsync(false);
            var controller = new ClassroomsController(mockRepo.Object);

            // Act
            var cut = await controller.Delete(classroomInDb.Id);
            var result = cut as BadRequestResult;

            result.StatusCode.Should().Be(400);
        }

        private static  Classroom TestClassroom()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture.Create<Classroom>();
        }


        private static IQueryable<Classroom> GetTestClassrooms()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture.CreateMany<Classroom>(5).AsQueryable();
        }
    
}
}
