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
        public async Task AddNewCadetWithExistingClassroom_ReturnNewCadet()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'name': 'Васечкин В.В.', 'classroom' : { 'id': 1, 'name': '7Б'}  }";
            var expectedCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            var addedCadet = await sut.AddCadet(expectedCadet);
            var actualCadet = await sut.FindCadetById(addedCadet.Id);

            Assert.Equal(3, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal(expectedCadet.Name, actualCadet.Name);
            Assert.Equal(expectedCadet.Classroom.Id, actualCadet.Classroom.Id);
            Assert.Equal(expectedCadet.Classroom.Name, actualCadet.Classroom.Name);
        }

        [Fact]
        public async Task AddNewCadetWithNewClassroom_ReturnNewCadetWithNewClassroom()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'name': 'Смирнов С.С.', 'classroom' : { 'name': '9Г'}  }";
            var expectedCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            var addedCadet = await sut.AddCadet(expectedCadet);
            var actualCadet = await sut.FindCadetById(addedCadet.Id);

            Assert.Equal(3, sut.GetCadets().Count());
            Assert.Equal(3, sut.GetClassroom().Count());
            Assert.Equal(expectedCadet.Name, actualCadet.Name);
            Assert.Equal(expectedCadet.Classroom.Name, actualCadet.Classroom.Name);
        }

        [Fact]
        public async Task UpdateCadetNameOnly_ReturnCadetWithupdatedName()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 1, 'name': 'Обновлено!', 'classroom' : { 'id': 1, 'name': '7Б'}  }";
            var expectedCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            var updatedCadet = await sut.UpdateCadet(expectedCadet);
            var actualCadet = await sut.FindCadetById(updatedCadet.Id);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal(expectedCadet.Id, actualCadet.Id);
            Assert.Equal(expectedCadet.Name, actualCadet.Name);
            Assert.Equal(expectedCadet.Classroom.Id, actualCadet.Classroom.Id);
            Assert.Equal(expectedCadet.Classroom.Name, actualCadet.Classroom.Name);
        }

        [Fact]
        public async Task UpdateCadetClassroom_ReturnCadetWithUpdatedClassroom()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 1, 'name': 'Петров П.П.', 'classroom' : { 'id': 2, 'name': '8А'}  }";
            var expectedCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            var updatedCadet = await sut.UpdateCadet(expectedCadet);
            var actualCadet = await sut.FindCadetById(updatedCadet.Id);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal(expectedCadet.Id, actualCadet.Id);
            Assert.Equal(expectedCadet.Name, actualCadet.Name);
            Assert.Equal(expectedCadet.Classroom.Id, actualCadet.Classroom.Id);
            Assert.Equal(expectedCadet.Classroom.Name, actualCadet.Classroom.Name);
        }

        [Fact]
        public async Task UpdateCadetClassroomWithNotExistingClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 1, 'name': 'Петров П.П.', 'classroom' : { 'id': 3, 'name': 'не существующий!'}  }";
            var cadetWithNotExistingClassrom = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.UpdateCadet(cadetWithNotExistingClassrom);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Проблеммы с базой данных", assertExeption.Message);
        }

        [Fact]
        public async Task UpdateNotExistingCadet_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 3, 'name': 'не существующий!', 'classroom' : { 'id': 2, 'name': '8А'}  }";
            var notExistingCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.UpdateCadet(notExistingCadet);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Проблеммы с базой данных", assertExeption.Message);
        }

        [Fact]
        public async Task AddEmptyCadet_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ }";
            var emptyCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.AddCadet(emptyCadet);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Проблеммы с базой данных", assertExeption.Message);
        }

        [Fact]
        public async Task AddCadetWhithoutName_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{'classroom' : { 'id': 1, 'name': '7Б'}  }";
            var cadetWithoutName = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.AddCadet(cadetWithoutName);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Проблеммы с базой данных", assertExeption.Message);
        }

        [Fact]
        public async Task AddCadetWhithoutClassroom_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'name': 'Васечкин В.В.' }";
            var cadetWithoutClassroom = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.AddCadet(cadetWithoutClassroom);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Проблеммы с базой данных", assertExeption.Message);
        }
    }
}
