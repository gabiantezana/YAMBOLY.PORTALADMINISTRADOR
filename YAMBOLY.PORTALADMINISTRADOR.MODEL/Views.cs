//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YAMBOLY.PORTALADMINISTRADOR.MODEL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Views
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Views()
        {
            this.RolesViews = new HashSet<RolesViews>();
        }
    
        public int ViewId { get; set; }
        public string ViewName { get; set; }
        public string ViewCode { get; set; }
        public int ViewGroupId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RolesViews> RolesViews { get; set; }
        public virtual ViewGroup ViewGroup { get; set; }
    }
}
