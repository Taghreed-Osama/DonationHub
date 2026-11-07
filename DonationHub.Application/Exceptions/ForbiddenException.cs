using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Application.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }
}
