using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class CatalogData
    {
        public decimal Id { get; set; }
        public decimal BookId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public Nullable<decimal> ReleaseYear { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
    }

    class CatalogDataExpanded
    {
        public decimal Id { get; set; }
        public decimal BookId { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public Nullable<decimal> ReleaseYear { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public Nullable<decimal> WholesalePrice { get; set; }
        public Nullable<decimal> SoldQuantity { get; set; }
        public Nullable<decimal> StorageQuantity { get; set; }

    }

    class BookData
    {
        public decimal Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public Nullable<decimal> ReleaseYear { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
    }

    class StoreData
    {
        public decimal Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
    }
}
