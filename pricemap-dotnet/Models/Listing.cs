using Newtonsoft.Json;
using System;

namespace pricemap.Models
{
    public class Listing
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("postal_address")]
        public PostalAddress PostalAddress { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("building_type")]
        public RealEstateListingBuildingType BuildingType { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("surface_area_m2")]
        public int SurfaceAreaM2 { get; set; }

        [JsonProperty("rooms_count")]
        public int RoomsCount { get; set; }

        [JsonProperty("bedrooms_count")]
        public int BedroomsCount { get; set; }

        [JsonProperty("contact_phone_number")]
        public string ContactPhoneNumber { get; set; }
    }

    public enum RealEstateListingBuildingType
    {
        [System.Runtime.Serialization.EnumMember(Value = @"STUDIO")]
        STUDIO = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"APARTMENT")]
        APARTMENT = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"HOUSE")]
        HOUSE = 2,

    }
}
