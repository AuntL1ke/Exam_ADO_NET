using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public enum Genre
    {
        Fiction,
        NonFiction,
        Mystery,
        Romance,
        ScienceFiction,
        Fantasy,
        Thriller,
        Horror,
        Biography,
        History,
        Poetry,
        Drama,
        Other
    }

    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int? Pages { get; set; }
        public int? Year { get; set; }
        public int? Cost { get; set; }
        public int? Price { get; set; }

        public bool? IsPart { get; set; }
        public Genre? Genre { get; set; }
        public int? IsDiscount { get; set; }
        public bool? IsNew { get; set; }
    }

    public class Purchase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int BookId { get; set; }

        public int Total {  get; set; }
        public Book Book { get; set; }
        public DateTime DateTime { get; set; }
    }
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }


}
