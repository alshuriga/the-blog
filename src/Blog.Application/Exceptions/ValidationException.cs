using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> _errors { get; private set; }
        public ValidationException(List<string> errors)
        {
            _errors = errors;   
        }
    }
}
