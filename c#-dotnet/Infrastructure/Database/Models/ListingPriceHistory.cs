using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace listingapi.Infrastructure.Database.Models
{
    [Table("listingpricehistory")]
    public class ListingPriceHistory
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column("listing_priceid")]
        public int ListingPriceId { get; set; }
        [Required]
        [Column("updated_date")]
        public DateTime UpdateDate { get; set; }
        [Required]
        [Column("price")]
        public double Price { get; set; }
    }
}
