using DAL.Entities;

namespace DAL.Core
{
    /// <summary>
    /// Класс-заглушка для добавления базовых сущностей
    /// </summary>
    internal class BaseEntitiesAddition
    {
        /// <summary>
        /// хеш от 's'
        /// </summary>
        private static string passHashS =
            "pIg20fAaHpUDZxibspj/8OxrP5kgjxYxpqn+jL4pNvEDeA+zbmItcVo1GpIS4ELmf3O0ELkV47qgkYeemrN2y/Fc6ZJfFdnDB2QH+vzVowG0kFTylw6GZdRvJSuuXTuyvdFerxvnNP+hM6mD9GGo+BUJxMb61G82tblDL5kowIfF+ihSQotRUwZTMHHUeI9D9x0t46byg6h+KHFHkkau9FaE5k/TEC22VdOkOII7CpU+D3JhSTMCUeBAVUmdZMpB";

        /// <summary>
        /// Добавляет в БД базовые сущности вида Login = s{i}; Password = s;
        /// </summary>
        /// <param name="dataContext">БД куда добавлять</param>
        /// <param name="count">кол-во сущностей</param>
        public static void Add(DataContext dataContext, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                dataContext.Students.Add(new Student() {
                    Login = $"s{i}",
                    PasswordHash = passHashS,
                    FirstName = "Егор",
                    SecondName = "Егоров",
                    ThirdName = "Егорович",
                    PhoneNumber = "89999999999",

                });
                dataContext.Tutors.Add(new Tutor()
                {
                    Login = $"t{i}",
                    PasswordHash = passHashS,
                    FirstName = "Анатолий",
                    SecondName = "Анатольев",
                    ThirdName = "Анатольевич",
                    AboutMyself = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Blanditiis cum debitis dignissimos eum ex hic inventore ipsam libero perspiciatis quis reiciendis, tempore vero voluptatum. At ea eos error iste magni neque rerum unde voluptatum? Consectetur eaque fugiat ipsum, laudantium magni necessitatibus suscipit temporibus unde vel voluptas! Aliquid assumenda harum illum impedit modi nesciunt quo. ",
                    BirthDate = new DateTime(2003, 05, 13),
                    Region = "Екатеринбург",
                    Sex = Entities.Enums.Sex.Male,
                    PhoneNumber = "89999999999",
                }) ;
            }
        }
    }
}
