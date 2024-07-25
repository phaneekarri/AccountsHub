using System;

namespace CustomerApi.Dto;

public record GetClient (string FirstName, string LastName, DateOnly DOB, int Id);

public record CreateClient (string FirstName, string LastName, DateOnly DOB);

public record UpdateClient (string FirstName, string LastName, DateOnly DOB);
