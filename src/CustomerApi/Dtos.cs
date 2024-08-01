using System;
using System.Collections.Generic;

namespace CustomerApi.Dto;

public class GetClient {
    public int Id {get; set;} 
    public string FirstName{get; set;} 
    public string LastName{get; set;} 
    public DateOnly DOB {get; set;}
    public int Age {get; set;}
}
public class  CreateClient {
    public string FirstName {get; set;} 
    public string LastName {get; set;} 
    public DateOnly DOB {get; set;}  

}
public class  UpdateClient {
    public string FirstName {get; set;} 
    public string LastName {get; set;} 
    public DateOnly DOB {get; set;}  

}

public class GetAccountOwner
{
   public int Id {get; set;} 
   public int ClientId {get; set;}
   public int AccountOwnerType{get; set;}
}
public class CreateAccountOwner
{
    public int AccountOwnerType {get; set;}
    public  int ClientId {get; set;} 
}
public class UpdateAccountOwner
{ 
    public int Id {get; set;}  
    public int AccountOwnerType {get; set;} 
};

public class GetAccount {
    public int Id{get; set;} 
    public string Title {get; set;}  
    public IEnumerable<GetAccountOwner> Owners {get; set;}
}
public class CreateAccount 
{ 
    public string Title {get; set;}  
    public IEnumerable<CreateAccountOwner> Owners {get; set;} 
}
public class UpdateAccount 
{
    public string Title {get; set;} 
}

