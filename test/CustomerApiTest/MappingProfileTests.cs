using CustomerApi;
using CustomerApi.Dto;
using CustomerEntities.Models;

namespace CustomerApiTest;

public class MappingExtensionsTests
{
        
        #region Client Mappings
        [Fact]
        public void Verify_CreateClient_Client()
        {     
              //Arrange
              CreateClient setUpClient = new CreateClient {
                 FirstName = "Test First Name", 
                 LastName = "Test Last Name",  
                 DOB = new DateOnly(2000,02,01)
                 };
              
              //Act
               Client actual = setUpClient.ToClient();
               Client expectedClient = new Client
                            {  
                               FirstName = "Test First Name",
                               LastName = "Test Last Name",  
                               DOB = new DateOnly(2000,02,01)
                            };
               //Assert                            
              Assert.Equal(expectedClient.FirstName, actual.FirstName);
              Assert.Equal(expectedClient.LastName, actual.LastName);
              Assert.Equal(expectedClient.DOB, actual.DOB);
        }
        [Fact]
        public void Verify_UpdateClient_Client()
        {
              //Arrange
              UpdateClient setUpClient = new UpdateClient {
                 FirstName = "Test First Name", 
                 LastName = "Test Last Name",  
                 DOB = new DateOnly(2000,02,01)
                 };
              Client client = new Client();
              
              //Act
               client.UpdateFrom(setUpClient);
               
               //Assert                            
              Assert.Equal("Test First Name", client.FirstName);
              Assert.Equal("Test Last Name", client.LastName);
              Assert.Equal(new DateOnly(2000,02,01), client.DOB);
        }

        [Fact]
        public void Verify_Client_To_GetClient()
        {
              //Arrange
              Client setUpClient = new Client {
                 Id = 1, FirstName = "Test First Name", 
                 LastName = "Test Last Name",  
                 DOB = new DateOnly(2000,02,01)
                 };
              
              //Act
               GetClient actual = setUpClient.ToGetClient();
               GetClient expectedClient = new GetClient
                            { 
                               Id =1, 
                               FirstName = "Test First Name",
                               LastName = "Test Last Name",  
                               DOB = new DateOnly(2000,02,01),
                               Age = DateTime.Now.Year - setUpClient.DOB.Year
                            };
               //Assert                            
              Assert.Equal(expectedClient.FirstName, actual.FirstName);
              Assert.Equal(expectedClient.LastName, actual.LastName);
              Assert.Equal(expectedClient.DOB, actual.DOB);
              Assert.Equal(expectedClient.Age, actual.Age);                                
        }
       #endregion

       #region Account Mappings


       [Fact]
       public void Verify_AccountOwner_To_GetAccountOwner()
    {
        //Arrange
        AccountOwner setUp = new AccountOwner { Id = 1,  };
        setUp.Update(new Client() {Id = 1});
        //Act
        var actual = setUp.ToGetAccountOwner();
        
        //Assert
        var expected = new GetAccountOwner { Id = 1, ClientId = 1,  };
        Assert.NotNull(actual);
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.ClientId, actual.ClientId);        
    }

      [Fact]
      public void Verify_CreateAccount_Account(){
         //Arrange
         CreateAccount setUp = new CreateAccount{
             Title = "Test Title"
         };
         //Act
         var actual = setUp.ToAccount();

         //Assert
         Assert.NotNull(actual);
         Assert.Equal("Test Title", actual.Title);
         Assert.NotNull(actual.AccountOwners);
      }

      [Fact]
      public void Verify_CreateAccountOwner_AccountOwner()
    {
        //Arrange
        CreateAccountOwner setUp = new CreateAccountOwner
        {
            Id = 1,
        };
        //Act
        AccountOwner actual = new AccountOwner();
        actual.Update(new Client { Id = 1 });
        //
        AccountOwner expected = new AccountOwner();
        expected.Update(new Client { Id = 1 });
        Assert.NotNull(actual);
        Assert.Equal(expected.ClientId, actual.ClientId);
    }

      [Fact]
      public void Verify_Account_To_GetAccount()
   {
          //Arrange

            Account setUp = new Account {
               Id  = 1, Title = "Test Title", 
               AccountOwners = new List<AccountOwner>()               
            };
            var accountOwner1 = new AccountOwner {Id =1, IsActive = true};
            accountOwner1.Update(new Client{Id = 1});
            setUp.AccountOwners.Add(accountOwner1);
            var accountOwner2 = new AccountOwner {Id =2, IsActive = true};
            accountOwner2.Update(new Client{Id = 2});
            setUp.AccountOwners.Add(accountOwner2);
            var accountOwner3 = new AccountOwner {Id = 3, IsActive = true};
            accountOwner3.Update(new Client{Id = 3});
               setUp.AccountOwners.Add(accountOwner3);
          //Act
            var actual = setUp.ToGetAccount();
            
          //Assert
          var expectedOwners = new List<GetAccountOwner>{            
                  new GetAccountOwner {Id = 1, ClientId = 1, },
                  new GetAccountOwner {Id = 2, ClientId = 2, },
                  new GetAccountOwner {Id = 3, ClientId = 3, }               
          };
          Assert.NotNull(actual);
          Assert.Equal(1, actual.Id);
          Assert.Equal("Test Title", actual.Title);
          Assert.NotNull(actual.Owners);
          Assert.Equal(3, actual.Owners.Count()); 
          var actualOwnersList = actual.Owners.ToList();
          for(int i= 0; i<3; i++) {
              Assert.Equal(expectedOwners[i].Id, actualOwnersList[i].Id);
              Assert.Equal(expectedOwners[i].ClientId, actualOwnersList[i].ClientId);
          }
   }
       
   #endregion
}
