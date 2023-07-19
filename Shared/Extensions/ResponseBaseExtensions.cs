using System;
using Platform.Shared.Models;

namespace Platform.Shared.Extensions
{
    public static class ResponseBaseExtensions
    {


        public static string ToErrorMessage(this ResponseBase response)
        {
            if (response == null)
            {
                return string.Empty;
            }

            if (response.Success)
            {
                return string.Empty;
            }


            var sb = new System.Text.StringBuilder();
            foreach (var erro in response.ValidationResults)
            {
                sb.AppendLine($"{erro.ErrorMessage}");
            }

            foreach (var erro in response.Errors)
            {
                sb.AppendLine($"{erro.Value}");
            }

            return sb.ToString();
        }

    }
}
