using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Controllers.Model
{
    public class ValidationFailedResponse
    {
        public ValidationFailedResponse(IEnumerable<ValidationError> validationErrors) => Errors = validationErrors;
        public ValidationFailedResponse(ValidationError validationError) => Errors = new[] { validationError };

        public IEnumerable<ValidationError> Errors { get; set; }
    }
}
