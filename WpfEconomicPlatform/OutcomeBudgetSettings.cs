//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfEconomicPlatform
{
    using System;
    using System.Collections.Generic;
    
    public partial class OutcomeBudgetSettings
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int totalAmount { get; set; }
        public int categoryOutcomeId { get; set; }
        public System.DateTime dateStart { get; set; }
        public System.DateTime dateEnd { get; set; }
    
        public virtual CategoriesOutcome CategoriesOutcome { get; set; }
        public virtual Users Users { get; set; }
    }
}
