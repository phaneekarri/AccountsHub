using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Dto;


namespace CustomerApi.Interfaces;

public interface IClientService
{
   Task<IEnumerable<GetClient>> GetAll();

   Task<GetClient>GetBy(int Id);

   Task<int> Create(CreateClient client);
   Task<bool> Patch(int Id, UpdateClient dto);
   Task<bool> Update(int Id, UpdateClient dto);
   Task<bool> Delete(int id);
}
