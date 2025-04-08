using System;
using System.Collections.Generic;
using System.Data;

namespace WpfEconomicPlatform.entities
{
    public class IncomesOutcomesDTO
    {
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        
    }
}