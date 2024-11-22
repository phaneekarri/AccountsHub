using System;
using System.Data.Common;
using InfraEntities.ModelType;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CustomerEntities.Models.Types
{
    public class ContactType : ModelType 
    {
        public ContactType(){}
        protected ContactType(int id, string description){
            Id = id;
            Description = description;
        }
        protected ContactType(ModelType modelType){
            Id = modelType.Id;
            Description = modelType.Description;
            IsActive = modelType.IsActive;
        }

    }

    public sealed class PrimaryContact: ContactType
    {
        public PrimaryContact():base(Activator.CreateInstance<Primary>()) {}
    }

    public sealed class SecondaryContact: ContactType
    {
        public SecondaryContact():base(Activator.CreateInstance<Secondary>()) {}
    }
}
