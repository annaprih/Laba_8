using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba7
{
    class ValidatAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string a = value as string;
            if (a.Length > 10)
            {
                this.ErrorMessage = "Длинная строка";
                return false;
            }
            return true;
        }
    }
}
