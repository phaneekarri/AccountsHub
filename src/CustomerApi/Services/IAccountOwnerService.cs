
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Dto;

namespace CustomerApi;

public interface IAccountOwnerService
{
  Task<bool> CreateOwnersByAccount(int Id, IEnumerable<CreateAccountOwner> Owners );
  Task<bool>DeleteAccountOwnersByClient(int AccountId, int ClientId);
  Task<bool> DeleteAccountOwners(int AccountId);
}
