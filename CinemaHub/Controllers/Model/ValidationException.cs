using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class ValidationException : Exception
    {
        public ValidationException(IEnumerable<ValidationError> validationErrors) => ValidationErrors = validationErrors;
        public ValidationException(ValidationError validationError) => ValidationErrors = new[] { validationError };

        public IEnumerable<ValidationError> ValidationErrors { get; set; }
    }
}
