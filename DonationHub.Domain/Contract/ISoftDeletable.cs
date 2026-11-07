using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Contract
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }

        public void Delete()
        {
            IsDeleted = true;
            DateDeleted = DateTimeOffset.UtcNow;
        }

        public void UndoDelete()
        {
            IsDeleted = false;
            DateDeleted = null;
        }
    }
}
