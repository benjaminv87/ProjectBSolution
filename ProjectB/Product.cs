//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public int ProductID { get; set; }
        public string Naam { get; set; }
        public Nullable<int> Marge { get; set; }
        public Nullable<int> Inkoopprijs { get; set; }
        public string Eenheid { get; set; }
        public Nullable<int> BTW { get; set; }
        public Nullable<int> LeverancierID { get; set; }
        public Nullable<int> CategorieID { get; set; }
    
        public virtual BestellingProduct BestellingProduct { get; set; }
        public virtual Categorie Categorie { get; set; }
        public virtual Leverancier Leverancier { get; set; }
    }
}