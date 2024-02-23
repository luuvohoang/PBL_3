using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerShop
{
    public class KhachHang
    {
        public int IDKhachHang { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }

        public override string ToString()
        { 
            return $"{Ho} {Ten} {Email} {SDT}";
        }
    }
}

