using AutoMapper;
using CustomerApi;
using CustomerApi.Dto;
using CustomerEntities.Models;

namespace CustomerApiTest;

[TestFixture]
public class MappingProfileTests
{
        private IMapper _mapper;
        private MapperConfiguration _mapperConfiguration;

        [SetUp]
        public void Setup()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _mapperConfiguration.CreateMapper();
        }
        
        #region Client Mappings
        [Test]
        public void Verify_CreateClient_Client()
        {     
              //Arrange
              CreateClient setUpClient = new CreateClient {
                 FirstName = "Test First Name", 
                 LastName = "Test Last Name",  
                 DOB = new DateOnly(2000,02,01)
                 };
              
              //Act
               Client actual = _mapper.Map<Client>(setUpClient);
               Client expectedClient = new Client
                            {  
                               FirstName = "Test First Name",
                               LastName = "Test Last Name",  
                               DOB = new DateOnly(2000,02,01)
                            };
               //Assert                            
              Assert.That(actual.FirstName , Is.EqualTo(expectedClient?.FirstName), "FirstName not matching");
              Assert.That(actual.LastName , Is.EqualTo(expectedClient?.LastName), "LastName not matching");
              Assert.That(actual.DOB , Is.EqualTo(expectedClient?.DOB), "DOB not matching");
        }
        [Test]
        public void Verify_UpdateClient_Client()
        {
              //Arrange
              UpdateClient setUpClient = new UpdateClient {
                 FirstName = "Test First Name", 
                 LastName = "Test Last Name",  
                 DOB = new DateOnly(2000,02,01)
                 };
              
              //Act
               Client actual = _mapper.Map<Client>(setUpClient);
               Client expectedClient = new Client
                            {  
                               FirstName = "Test First Name",
                               LastName = "Test Last Name",  
                               DOB = new DateOnly(2000,02,01)
                            };
               //Assert                            
              Assert.That(actual.FirstName , Is.EqualTo(expectedClient?.FirstName), "FirstName not matching");
              Assert.That(actual.LastName , Is.EqualTo(expectedClient?.LastName), "LastName not matching");
              Assert.That(actual.DOB , Is.EqualTo(expectedClient?.DOB), "DOB not matching");
        }

        [Test]
        public void Verify_Client_To_GetClient()
        {
              //Arrange
              Client setUpClient = new Client {
                 Id = 1, FirstName = "Test First Name", 
                 LastName = "Test Last Name",  
                 DOB = new DateOnly(2000,02,01)
                 };
              
              //Act
               GetClient actual = _mapper.Map<Client,GetClient>(setUpClient);
               GetClient expectedClient = new GetClient
                            { 
                               Id =1, 
                               FirstName = "Test First Name",
                               LastName = "Test Last Name",  
                               DOB = new DateOnly(2000,02,01),
                               Age = 25
                            };
               //Assert                            
              Assert.That(actual.FirstName , Is.EqualTo(expectedClient?.FirstName), "FirstName not matching");
              Assert.That(actual.LastName , Is.EqualTo(expectedClient?.LastName), "LastName not matching");
              Assert.That(actual.DOB , Is.EqualTo(expectedClient?.DOB), "DOB not matching");
              Assert.That(actual.Age , Is.EqualTo(expectedClient?.Age), "Age not matching");                                
        }
       #endregion

       #region Account Mappings


       [Test]
       public void Verify_AccountOwner_To_GetAccountOwner()
    {
        //Arrange
        AccountOwner setUp = new AccountOwner { Id = 1,  };
        setUp.Update(new Client() {Id = 1});
        //Act
        var actual = _mapper.Map<GetAccountOwner>(setUp);
        
        //Assert
        var expected = new GetAccountOwner { Id = 1, ClientId = 1,  };
        GetAccountOwnerAssertions(actual, expected );
    }

       private void GetAccountOwnerAssertions(GetAccountOwner? actual, GetAccountOwner? expected)
    {
        Assert.That(actual != null, "Value is null");
        Assert.That(actual?.Id == expected?.Id, "Id is not matching");
        Assert.That(actual?.ClientId == expected?.Id, "Client ID is not matching ");        
    }

      [Test]
      public void Verify_CreateAccount_Account(){
         //Arrange
         CreateAccount setUp = new CreateAccount{
             Title = "Test Title"
         };
         //Act
         var actual = _mapper.Map<Account>(setUp);

         //Assert
         var expectedOwners = new List<AccountOwner>{         };
         Assert.That(actual != null, "Value is null");
         Assert.That(actual?.Title == "Test Title", "Id not matching");
         Assert.That(actual?.AccountOwners != null, "Owners is null");
         Assert.That(actual?.AccountOwners.Count() == 3, "Owners count not matching"); 
      }

      [Test]
      public void Verify_CreateAccountOwner_AccountOwner()
    {
        //Arrange
        CreateAccountOwner setUp = new CreateAccountOwner
        {
            ClientId = 1,
        };
        //Act
        AccountOwner actual = _mapper.Map<AccountOwner>(setUp);
        //
        AccountOwner expected = new AccountOwner ();
        CreateAccountOwnerAssertions(actual, expected);
    }

      private void CreateAccountOwnerAssertions(AccountOwner? actual, AccountOwner? expected)
    {
        Assert.That(actual != null, "value is null");
        Assert.That(actual?.ClientId == expected?.ClientId, "ClientId is not matching");
    }

      [Test]
      public void Verify_Account_To_GetAccount()
   {
          //Arrange
            Account setUp = new Account {
               Id  = 1, Title = "Test Title", 
               AccountOwners = new List<AccountOwner>{

               }
            };
          //Act
            var actual = _mapper.Map<GetAccount>(setUp);
            
          //Assert
          var expectedOwners = new List<GetAccountOwner>{            
                  new GetAccountOwner {Id = 1, ClientId = 1, },
                  new GetAccountOwner {Id = 2, ClientId = 2, },
                  new GetAccountOwner {Id = 3, ClientId = 3, }               
          };
          Assert.That(actual != null, "Value is null");
          Assert.That(actual?.Id == 1, "Id not matching");
          Assert.That(actual?.Title == "Test Title", "Id not matching");
          Assert.That(actual?.Owners != null, "Owners is null");
          Assert.That(actual?.Owners.Count() == 3, "Owners count not matching"); 
          var actualOwnersList = actual?.Owners.ToList();
          for(int i= 0; i<3; i++) GetAccountOwnerAssertions(actualOwnersList?[i],expectedOwners[i]);
   }
       
   #endregion
}
