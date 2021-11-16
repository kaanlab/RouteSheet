using Newtonsoft.Json;
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
        public async Task AddLesson_NewLesson_ReturnNewEntity()
        {
            IAppRepository sut = new AppRepository(AppDbContextInMemory());
            var cadet = await sut.FindCadetById(1);
            var appUser = sut.FindUserByUserName("petrpku");
            var expectedLesson = new Lesson {
                AppUser = appUser,
                Cadet = cadet,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Hour = 2,
                Prioriy = Priority.Normal,
                Title = "Программирование"
            };

            var addedLesson = await sut.AddLesson(expectedLesson);
            var actualLesson = await sut.FindLessonById(addedLesson.Id);

            Assert.Equal(expectedLesson.AppUser.Id, expectedLesson.AppUser.Id);
            Assert.Equal(expectedLesson.Cadet.Id, expectedLesson.Cadet.Id);
            Assert.Equal(expectedLesson.Id, expectedLesson.Id);
        }
    }
}
