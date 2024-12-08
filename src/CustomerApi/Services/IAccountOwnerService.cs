
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApi.Dto;

namespace CustomerApi;

public interface IAccountOwnerService
{
  Task<bool> AddOwnersToAccount(int Id, IEnumerable<CreateAccountOwner> Owners );
  Task<bool> DeleteOwnerToAccount(int AccountId, int ClientId);
  Task<bool> DeleteOwnersToAccount(int AccountId);
}
