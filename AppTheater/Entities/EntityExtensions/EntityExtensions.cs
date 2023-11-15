using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppTheater.Entities.EntityExtensions
{
    public static class EntityExtensions
    {
        public static T? Copy<T>(this T itemCopy) where T : IEntity
        {
            var json = JsonSerializer.Serialize(itemCopy);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
