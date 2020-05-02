using System;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace CadCliSearch
{
    [SerializePropertyNamesAsCamelCase]
    public class Cliente
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsFilterable]
        public string Id { get; } = Guid.NewGuid().ToString("N");

        [IsFilterable, IsSortable]
        public string Name { get; set; }

        [IsFilterable, IsSortable]
        public string Country { get; set; }


        [IsFilterable, IsSortable]
        public DateTime DateCreated { get; } = DateTime.UtcNow;
    }
}