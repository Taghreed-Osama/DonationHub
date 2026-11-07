using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTimeOffset CreatedAt { get; protected set; }

        protected BaseEntity() { }

        protected BaseEntity(DateTimeOffset createdat)
        {
            Id = Guid.NewGuid();
            CreatedAt = createdat;
        }
    }
}
