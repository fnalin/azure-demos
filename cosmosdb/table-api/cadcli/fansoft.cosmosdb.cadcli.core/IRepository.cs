using System.Collections.Generic;
using System.Threading.Tasks;

namespace fansoft.cosmosdb.cadcli.core
{
    public interface IRepository
    {
         Task<Cliente> AddItemsToContainerAsync(Cliente cli);
         Task<IEnumerable<Cliente>> GetAllClientesAsync();
    }
}