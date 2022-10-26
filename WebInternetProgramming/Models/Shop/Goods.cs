using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebInternetProgramming.Models.Shop
{
    public class Goods
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Cost { get; set; }
        public int Amount { get; set; }
        public string Src { get; set; }
    }
}
