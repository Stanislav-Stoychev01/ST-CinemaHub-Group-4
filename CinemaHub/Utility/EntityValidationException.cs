using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Utility
{
    public class EntityValidationException : Exception
    {
        public EntityValidationException(string message): base(message)
        {
            
        }
    }
}
