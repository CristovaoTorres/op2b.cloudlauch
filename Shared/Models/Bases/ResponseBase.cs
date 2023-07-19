using System;
using System.Collections.Generic;

namespace Platform.Shared.Models
{
    public class ResponseBase
    {
        public ResponseBase()
        {
            Success = false;
            Errors = new Dictionary<string, string>();
            ValidationResults = new List<ValidationFailure>();
        }

        public bool Success { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public List<ValidationFailure> ValidationResults { get; set; }
    }

    public class ResponseBase<T> : ResponseBase
    {
        public ResponseBase() : base()
        {

        }

        public T Item { get; set; }
    }

    public class ValidationFailure
    {
        public ValidationFailure()
        {
        }


        /// <summary>
        /// The error message
        /// </summary>
        public string ErrorMessage { get; set; }

        public string ErrorCode { get; set; }

    }
}
