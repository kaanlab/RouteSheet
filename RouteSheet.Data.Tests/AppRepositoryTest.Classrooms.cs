using Newtonsoft.Json;
using RouteSheet.Data.Exceptions;
using RouteSheet.Data.Repositories;
using RouteSheet.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RouteSheet.Data.Tests
{
    public partial class AppRepositoryTest
    {
        [Fact(DisplayName ="Adding classroom should return new entity")]
        public async Task Classroom_Test_001()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'name': '5Г' }";
            var expectedClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            var addedClassroom = await sut.AddClassroom(expectedClassroom);
            var actualClassroom = await sut.FindClassroomById(addedClassroom.Id);

            Assert.Equal(3, sut.AllClassrooms().Count());
            Assert.Equal(expectedClassroom.Name, actualClassroom.Name);
            Assert.Equal(expectedClassroom.Id, actualClassroom.Id);
        }

        [Fact(DisplayName ="Adding empty classroom shouil return AppRepositoryException")]
        public async Task Classroom_Test_002()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ }";
            var emptyClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            Func<Task> atc = async () => await sut.AddClassroom(emptyClassroom);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllClassrooms().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task AddClassroom_NullClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            Func<Task> atc = async () => await sut.AddClassroom(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllClassrooms().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task UpdateClassroom_NullClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            Func<Task> atc = async () => await sut.UpdateClassroom(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllClassrooms().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task UpdateClassroom_UpdateName_ReturnUpdatedEntity()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{'id': 1, 'name': '8Б' }";
            var expectedClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            var addedClassroom = await sut.UpdateClassroom(expectedClassroom);
            var actualClassroom = await sut.FindClassroomById(addedClassroom.Id);

            Assert.Equal(2, sut.AllClassrooms().Count());
            Assert.Equal(expectedClassroom.Name, actualClassroom.Name);
            Assert.Equal(expectedClassroom.Id, actualClassroom.Id);
        }

        [Fact]
        public async Task UpdateClassroom_EmptyClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ }";
            var emptyClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            Func<Task> atc = async () => await sut.UpdateClassroom(emptyClassroom);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllClassrooms().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName ="Deleting existing classroom should return true")]
        public async Task DeleteClassroom_WithExistingClassroom_Return_True()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 1, 'name': '7Б' }";
            var expectedClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            var result = await sut.DeleteClassroom(expectedClassroom.Id);

            var deletedCadet = await sut.FindClassroomById(expectedClassroom.Id);

            Assert.Equal(1, sut.AllClassrooms().Count());
            Assert.True(result);
            Assert.Null(deletedCadet);
        }        

        [Fact(DisplayName ="Deleting wrong classroom should return AppRepositortyException")]
        public async Task DeleteClassroom_WithWrongClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 5, 'name': 'не существующий!' }";
            var wrongClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            Func<Task> atc = async () => await sut.DeleteClassroom(wrongClassroom.Id);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllClassrooms().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }
    }
}
