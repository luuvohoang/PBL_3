using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerShop
{
    public class HangMuc
    {
        public int IDHangMuc { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }

        public override string ToString()
        {
            return $"{Ten} {MoTa}";
        }
    }
}
