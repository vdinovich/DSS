namespace DTS.Models
{
    using System.Collections.Generic;

    public class MenuItem
    {
        public MenuItem() { Children = new List<MenuItem>(); }
        public int Id { get; set; }
        public string Header { get; set; }      // заголовок меню
        public string Url { get; set; }         // адрес ссылки
        public int? Order { get; set; }         // порядок следования пункта в подменю
        public int? ParentId { get; set; }      // ссылка на id родительского меню
        public MenuItem Parent { get; set; }    // родительское меню

        public ICollection<MenuItem> Children { get; set; }   // дочерние пункты меню
    }
}