using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Dto;

namespace CustomerApi;

public interface IAccountService
{
   Task<IEnumerable<GetAccount>> GetAll();

   Task<GetAccount>GetBy(int Id);

   Task<int> Create(CreateAccount client);
   Task<bool> Update(int Id, UpdateAccount dto);
   Task<bool> Delete(int id);
}
