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
    
    public partial class ViewGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ViewGroup()
        {
            this.Views = new HashSet<Views>();
        }
    
        public int ViewGroupId { get; set; }
        public string ViewGroupName { get; set; }
        public string ViewGroupCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Views> Views { get; set; }
    }
}