using System;
using System.Collections.Generic;

namespace Drs.Model.Shared
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<MenuItem> SubMenu { get; private set; }
        public int? ParentId { get; set; }
        public int Position { get; set; }
        public string Route { get; set; }

        public MenuItem()
        {
            SubMenu = new List<MenuItem>();
        }

        public MenuItem CopyMenu()
        {
            return new MenuItem
            {
                Id = Id,
                Area = Area,
                Controller = Controller,
                Action = Action,
                Title = Title,
                Icon = Icon,
                ParentId = ParentId,
                Position = Position,
                Route = String.Format("{0}/{1}/{2}", Area, Controller, Action)
            };
        }

    }
}