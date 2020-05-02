using System;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace CadCliSearch
{
    [SerializePropertyNamesAsCamelCase]
    public class Cliente
    {
        public string Id { get; set;}

        public string Name { get; set; }

        public string Country { get; set; }

        public DateTime DateCreated { get; set;}
    }
}