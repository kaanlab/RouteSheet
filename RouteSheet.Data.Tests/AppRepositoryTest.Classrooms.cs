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
        public async Task AddNewClassroom()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ 'name': '5Г' }";
            var expectedClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            var addedClassroom = await sut.AddClassroom(expectedClassroom);
            var actualClassroom = await sut.FindClassroomById(addedClassroom.Id);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(3, sut.GetClassroom().Count());
            Assert.Equal(expectedClassroom.Name, actualClassroom.Name);
            Assert.Equal(expectedClassroom.Id, actualClassroom.Id);
        }

        [Fact]
        public async Task AddEmptyClassroom_ShoudReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            string json = @"{ }";
            var emptyClassroom = await JsonConvert.DeserializeObjectAsync<Classroom>(json);

            Func<Task> atc = async () => await sut.AddClassroom(emptyClassroom);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryExeption>(atc);

            Assert.Equal(2, sut.GetCadets().Count());
            Assert.Equal(2, sut.GetClassroom().Count());
            Assert.Equal("Проблеммы с базой данных", assertExeption.Message);
        }
    }
}
