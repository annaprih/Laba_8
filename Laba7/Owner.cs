using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba7
{
    [Serializable]

    public class Owner
    {
       [Required(ErrorMessage = "Не указано имя пользователя")]
       [StringLength(10, MinimumLength = 3, ErrorMessage = "Коротокое имя")]
       [Validat]
        public string FLS { get; set; }
        public string DataOfBirthday { get; set; }

        public string PasportInf { get; set; }

        public override string ToString()
        {
            return this.FLS;
        }
    }
}
