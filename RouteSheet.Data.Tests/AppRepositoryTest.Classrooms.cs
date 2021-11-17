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
        [Fact]
        public async Task AddClassroom_NewClassroom_ReturnNewEntity()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'name': '5Г' }";
            var expectedClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            var addedClassroom = await sut.AddClassroom(expectedClassroom);
            var actualClassroom = await sut.FindClassroomById(addedClassroom.Id);

            Assert.Equal(3, sut.GetClassroom().Count());
            Assert.Equal(expectedClassroom.Name, actualClassroom.Name);
            Assert.Equal(expectedClassroom.Id, actualClassroom.Id);
        }

        [Fact]
        public async Task AddClassroom_EmptyClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ }";
            var emptyClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            Func<Task> atc = async () => await sut.AddClassroom(emptyClassroom);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task AddClassroom_NullClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            Func<Task> atc = async () => await sut.AddClassroom(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task UpdateClassroom_NullClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            Func<Task> atc = async () => await sut.UpdateClassroom(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetClassroom().Count());
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

            Assert.Equal(2, sut.GetClassroom().Count());
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

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task DeleteClassroom_WithExistingClassroom_ReturnDeletedEntity()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 1, 'name': '7Б' }";
            var expectedClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            var actualClassroom = await sut.DeleteClassroom(expectedClassroom);

            var deletedCadet = await sut.FindClassroomById(actualClassroom.Id);

            Assert.Equal(1, sut.GetClassroom().Count());
            Assert.Equal(expectedClassroom.Id, actualClassroom.Id);
            Assert.Null(deletedCadet);
        }

        [Fact]
        public async Task DeleteClassroom_NullClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());

            Func<Task> atc = async () => await sut.DeleteClassroom(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task DeleteClassroom_WithWrongClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 5, 'name': 'не существующий!' }";
            var wrongClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            Func<Task> atc = async () => await sut.DeleteClassroom(wrongClassroom);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }
    }
}
