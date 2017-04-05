using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba7
{
    [Serializable]
    public class Operation
    {
        public string TypeOfOperation { get; set; }
        [RegularExpression(@"[0-9]{2}")]
        public int Sum { get; set; }

        public string DateOfOperation { get; set; }

        public override string ToString()
        {
            return this.TypeOfOperation + " " + this.Sum;
        }
    }
}
