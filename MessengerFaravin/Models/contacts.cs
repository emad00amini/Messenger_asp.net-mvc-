//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MessengerFaravin.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class contacts
    {
        public int id { get; set; }
        public int listOwner_id { get; set; }
        public int contactUser_id { get; set; }
        public string name { get; set; }
        public string family { get; set; }
        public bool status { get; set; }
    
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
    }
}
