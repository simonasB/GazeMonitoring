using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GazeMonitoring.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SessionTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var inputValue = (DateTime)value;
            var isValid = true;

            /*if (!string.IsNullOrEmpty(inputValue))
            {
                isValid = inputValue.ToUpperInvariant() != "BANANA";
            }*/

            return isValid;
        }
    }
}
