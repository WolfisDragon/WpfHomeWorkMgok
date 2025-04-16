using System;
using System.Collections.Generic;
using System.Data;

namespace WpfEconomicPlatform.entities
{
    public class IncomesOutcomesDTO
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        
    }
}