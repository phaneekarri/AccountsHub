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
   Task<bool> Delete(int Id);

   Task<bool> AddOwners(int Id, IEnumerable<CreateAccountOwner> Owners );
   Task<bool> DeleteOwner(int Id, int ClientId);
   Task<bool> DeleteOwners(int Id);
}
