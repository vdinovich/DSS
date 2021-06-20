namespace DTS.Models
{
    using System.Collections.Generic;

    public class Init
    {
        public static IEnumerable<MenuItem> Seed()
        {
            return new List<MenuItem>
             {
                  new MenuItem{Id=1, Header = "Главная", Url = "/Home/Index", Order = 1},
                  new MenuItem{Id=2, Header = "О сайте", Url = "/Home/About", Order = 2},
                  new MenuItem{Id=3, Header = "Контакты", Url = "/Home/Contact", Order = 3},
                  new MenuItem{Id=4, Header = "Меню второго уровня 1", Url = "#", Order = 1, ParentId = 2},
                  new MenuItem{Id=5, Header = "Меню второго уровня 2", Url = "#", Order = 2, ParentId = 2},
                  new MenuItem{Id=6, Header = "Меню второго уровня 3", Url = "#", Order = 3, ParentId = 2},
            new MenuItem{Id=7, Header = "Меню третьго уровня 1", Url = "#",  Order = 1, ParentId = 4},
            new MenuItem{Id=8, Header = "Меню третьго уровня 2", Url = "#", Order = 2, ParentId = 4},
            new MenuItem{Id=9, Header = "Меню третьго уровня 3", Url = "#", Order = 3, ParentId = 4}
        };
        }
    }
}