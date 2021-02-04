﻿using LoanEntities.Models.Contacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanEntities.Configurations
{
    public class ClientAddressContactConfiguration : IEntityTypeConfiguration<ClientAddressContact>
    {
        public void Configure(EntityTypeBuilder<ClientAddressContact> builder)
        {

            builder.Property(x => x.Value)
                   .IsRequired();
            builder.Property(x => x.ContactType)
                   .IsRequired();
        }
    }
}