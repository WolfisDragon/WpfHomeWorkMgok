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
    
    public partial class CategoriesOutcome
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoriesOutcome()
        {
            this.OutcomeBudgetSettings = new HashSet<OutcomeBudgetSettings>();
            this.Outcomes = new HashSet<Outcomes>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public int userId { get; set; }
    
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutcomeBudgetSettings> OutcomeBudgetSettings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Outcomes> Outcomes { get; set; }
    }
}
