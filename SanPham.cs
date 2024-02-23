using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerShop
{
    public class SanPham
    {
        public int IDSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string MoTa { get; set; }
        public double GiaThanh { get; set; }
        public int SoLuong { get; set; }
        public int IDNhaPhanPhoi { get; set; }
        public int IDNhanVien { get; set; }


        public override string ToString()
        {
            return $"{TenSanPham} {MoTa}";
        }
    }
}
