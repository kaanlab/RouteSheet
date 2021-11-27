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
        public async Task AddLesson_CorrectLesson_ReturnNewEntity()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var cadet = await sut.FindCadetById(1);
            var appUser = sut.FindUserByUserName("petrpku");

            var expectedLesson = new Lesson
            {
                AppUser = appUser,
                Cadet = cadet,
                Date = DateTime.Now,
                Hour = 2,
                Prioriy = Priority.Normal,
                Title = "Программирование"
            };

            var addedLesson = await sut.AddLesson(expectedLesson);
            var actualLesson = await sut.FindLessonById(addedLesson.Id);

            Assert.Equal(3, sut.AllLessons().Count());
            Assert.Equal(expectedLesson.AppUser.Id, actualLesson.AppUser.Id);
            Assert.Equal(expectedLesson.Cadet.Id, actualLesson.Cadet.Id);
            Assert.Equal(expectedLesson.Id, actualLesson.Id);
        }

        [Fact]
        public async Task AddLesson_LessonWithoutUser_ReturnExeption()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var cadet = await sut.FindCadetById(1);

            var lessonWithoutUser = new Lesson
            {
                Cadet = cadet,
                Date = DateTime.Now,
                Hour = 2,
                Prioriy = Priority.Normal,
                Title = "Программирование"
            };

            Func<Task> atc = async () => await sut.AddLesson(lessonWithoutUser);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task AddLesson_LessonWithoutCadet_ReturnExeption()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var appUser = sut.FindUserByUserName("petrpku");

            var lessonWithoutUser = new Lesson
            {
                AppUser = appUser,
                Date = DateTime.Now,
                Hour = 2,
                Prioriy = Priority.Normal,
                Title = "Программирование"
            };

            Func<Task> atc = async () => await sut.AddLesson(lessonWithoutUser);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task AddLesson_LessonWithoutTitle_ReturnExeption()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var appUser = sut.FindUserByUserName("petrpku");
            var cadet = await sut.FindCadetById(1);

            var lessonWithoutTitle = new Lesson
            {
                AppUser = appUser,
                Cadet = cadet,
                Date = DateTime.Now,
                Hour = 2,
                Prioriy = Priority.Normal
            };

            Func<Task> atc = async () => await sut.AddLesson(lessonWithoutTitle);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task AddLesson_NullLesson_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());

            Func<Task> atc = async () => await sut.AddLesson(null);
            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact]
        public async Task UpdateLesson_ChangeTitle_ReturnUpdatedEntity()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var lessonInDb = await sut.FindLessonById(1);

            var updatedLesson = new Lesson
            {
                Id = lessonInDb.Id,
                AppUser = lessonInDb.AppUser,
                Cadet = lessonInDb.Cadet,
                Date = lessonInDb.Date,
                Hour = lessonInDb.Hour,
                Prioriy = lessonInDb.Prioriy,
                Title = "Программирование",

            };

            var expectedLesson = await sut.UpdateLesson(updatedLesson);
            var actualLesson = await sut.FindLessonById(1);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal(expectedLesson.AppUser.Id, actualLesson.AppUser.Id);
            Assert.Equal(expectedLesson.Cadet.Id, actualLesson.Cadet.Id);
            Assert.Equal(expectedLesson.Id, actualLesson.Id);
            Assert.Equal(expectedLesson.Title, actualLesson.Title);
        }

        [Fact]
        public async Task UpdateLesson_ChangeCadet_ReturnUpdatedEntity()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var lessonInDb = await sut.FindLessonById(1);
            var cadet = await sut.FindCadetById(2);

            var lessonWithNewCadet = new Lesson
            {
                Id = lessonInDb.Id,
                AppUser = lessonInDb.AppUser,
                Cadet = cadet,
                Date = lessonInDb.Date,
                Hour = lessonInDb.Hour,
                Prioriy = lessonInDb.Prioriy,
                Title = lessonInDb.Title
            };

            var expectedLesson = await sut.UpdateLesson(lessonWithNewCadet);
            var actualLesson = await sut.FindLessonById(1);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal(expectedLesson.AppUser.Id, actualLesson.AppUser.Id);
            Assert.Equal(expectedLesson.Cadet.Id, actualLesson.Cadet.Id);
            Assert.Equal(expectedLesson.Id, actualLesson.Id);
        }

        [Fact]
        public async Task UpdateLesson_ChangeUser_ReturnUpdatedEntity()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var lessonInDb = await sut.FindLessonById(1);
            var appUser = sut.FindUserByUserName("petrpku");

            var lessonWithNewUser = new Lesson
            {
                Id = lessonInDb.Id,
                AppUser = appUser,
                Cadet = lessonInDb.Cadet,
                Date = lessonInDb.Date,
                Hour = lessonInDb.Hour,
                Prioriy = lessonInDb.Prioriy,
                Title = lessonInDb.Title
            };

            var expectedLesson = await sut.UpdateLesson(lessonWithNewUser);
            var actualLesson = await sut.FindLessonById(1);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal(expectedLesson.AppUser.Id, actualLesson.AppUser.Id);
            Assert.Equal(expectedLesson.Cadet.Id, actualLesson.Cadet.Id);
            Assert.Equal(expectedLesson.Id, actualLesson.Id);
        }

        [Fact]
        public async Task UpdateLesson_ChangeUserWithNullUser_ReturnException()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var lessonInDb = await sut.FindLessonById(1);

            var lessonWithoutUser = new Lesson
            {
                Id = lessonInDb.Id,
                Cadet = lessonInDb.Cadet,
                Date = lessonInDb.Date,
                Hour = lessonInDb.Hour,
                Prioriy = lessonInDb.Prioriy,
                Title = lessonInDb.Title
            };

            Func<Task> atc = async () => await sut.UpdateLesson(lessonWithoutUser);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName ="Deleting wrong lesson should return AppRepositoryException")]
        public async Task DeleteLesson_001()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var cadet = await sut.FindCadetById(2);
            var appUser = sut.FindUserByUserName("petrpku");
            var wrongLesson = new Lesson
            {
                Id = 30,
                Cadet = cadet,
                Date = DateTime.Now,
                Hour = 2,
                Prioriy = Priority.Normal,
                Title = "Программирование",
                AppUser = appUser
            };

            Func<Task> atc = async () => await sut.DeleteLesson(wrongLesson.Id);

            var assertExeption = await Assert.ThrowsAsync<AppRepositoryException>(atc);

            Assert.Equal(2, sut.AllLessons().Count());
            Assert.Equal("Data layer problems, see details for more info", assertExeption.Message);
        }

        [Fact(DisplayName ="Deleting existing lesson should return true")]
        public async Task DeleteLesson_002()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var lessonInDb = await sut.FindLessonById(1);

            var expectedLesson = new Lesson
            {
                Id = lessonInDb.Id,
                Cadet = lessonInDb.Cadet,
                Date = lessonInDb.Date,
                Hour = lessonInDb.Hour,
                Prioriy = lessonInDb.Prioriy,
                Title = lessonInDb.Title,
                AppUser = lessonInDb.AppUser
            };

            var result = await sut.DeleteLesson(expectedLesson.Id);

            var deletedLesson = await sut.FindLessonById(expectedLesson.Id);

            Assert.Equal(1, sut.AllLessons().Count());
            Assert.True(result);
            Assert.Null(deletedLesson);
        }
    }
}
