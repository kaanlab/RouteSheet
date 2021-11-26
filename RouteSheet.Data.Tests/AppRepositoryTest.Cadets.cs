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
        [Fact(DisplayName = "Adding new cadet should return new entity")]
        public async Task AddCadet_001()
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

        [Fact(DisplayName = "Adding cadet with new classroom should return new entity")]
        public async Task AddCadet_002()
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

        [Fact(DisplayName = "Adding empty cadet should return AppRepositoryExeption")]
        public async Task AddCadet_003()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ }";
            var emptyCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.AddCadet(emptyCadet);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName = "Adding cadet without name should return AppRepositoryExeption")]
        public async Task AddCadet_004()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{'classroom' : { 'id': 1, 'name': '7Б'}  }";
            var cadetWithoutName = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.AddCadet(cadetWithoutName);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName = "Adding cadet without classroom should return AppRepositoryExeption")]
        public async Task AddCadet_005()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'name': 'Васечкин В.В.' }";
            var cadetWithoutClassroom = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.AddCadet(cadetWithoutClassroom);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName = "Adding cadet with no existing classroom should return AppRepositoryExeption")]
        public async Task AddCadet_006()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'name': 'Васечкин В.В.','classroom' : { 'id': 3, 'name': 'не существующий!'}   }";
            var cadetWithNotExistingClassroom = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.AddCadet(cadetWithNotExistingClassroom);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName = "Adding null cadet should return AppRepositoryExeption")]
        public async Task AddCadet_007()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());

            Func<Task> atc = async () => await sut.AddCadet(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName = "Updating cadet property name should return updated entity")]
        public async Task UpdateCadet_001()
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

        [Fact(DisplayName = "Updating cadet classroom property should return updated entity")]
        public async Task UpdateCadet_002()
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

        [Fact(DisplayName = "Updating cadet classroom property with no existing classroom should return AppRepositoryExeption")]
        public async Task UpdateCadet_003()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 1, 'name': 'Петров П.П.', 'classroom' : { 'id': 3, 'name': 'не существующий!'}  }";
            var cadetWithNotExistingClassroom = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.UpdateCadet(cadetWithNotExistingClassroom);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName = "Updating cadet with no existing cadet should return AppRepositoryExeption")]
        public async Task UpdateCadet_004()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 3, 'name': 'не существующий!', 'classroom' : { 'id': 2, 'name': '8А'}  }";
            var notExistingCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.UpdateCadet(notExistingCadet);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName = "Updating cadet with null cadet should return AppRepositoryExeption")]
        public async Task UpdateCadet_005()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());

            Func<Task> atc = async () => await sut.UpdateCadet(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName ="Deleting cadet with existing cadet should return deleted entity")]
        public async Task DeleteCadet_001()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 1, 'name': 'Петров П.П.', 'classroom' : { 'id': 1, 'name': '7Б'}  }";
            var expectedCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            var actualCadet = await sut.DeleteCadet(expectedCadet);

            var deletedCadet = await sut.FindCadetById(actualCadet.Id);

            Assert.Equal(1, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal(expectedCadet.Id, actualCadet.Id);
            Assert.Null(deletedCadet);
        }

        [Fact(DisplayName = "Deleting null cadet should return AppRepositoryExeption")]
        public async Task DeleteCadet_002()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());

            Func<Task> atc = async () => await sut.DeleteCadet(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName = "Deleting cadet with no existing cadet should return AppRepositoryExeption")]
        public async Task DeleteCadet_003()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'id': 3, 'name': 'не существующий!', 'classroom' : { 'id': 2, 'name': '8А'}  }";
            var wrongCadet = await JsonConvert.DeserializeObjectAsync<Cadet>(json);

            Func<Task> atc = async () => await sut.DeleteCadet(wrongCadet);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName ="Find cadet with wrong id should return null")]
        public async Task FindCadetById_001()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());

            var actualcadet = await sut.FindCadetById(10);

            Assert.Null(actualcadet);
        }
        [Fact(DisplayName ="Find cadet with existing cadet should return cadet entity")]
        public async Task FindCadetById_002()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var expectedCadet = new Cadet { Name = "Петров П.П.", Classroom = new Classroom { Name = "7Б" } };

            var actualCadet = await sut.FindCadetById(1);

            Assert.Equal(expectedCadet.Name, actualCadet.Name);
            Assert.Equal(expectedCadet.Classroom.Name, actualCadet.Classroom.Name);
        }
    }
}
