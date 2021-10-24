namespace LocationApi
{
    public class Location
    {
        public bool Found { get; set; }
        public string AddresLine { get; set; } // "addressLine": "Calle Nueva Palmira 1964",
        public string CountryRegion { get; set; } // "countryRegion": "Uruguay",
        public string FormattedAddress { get; set; } // "formattedAddress": "Calle Nueva Palmira 1964, Montevideo, 11800",
        public string Locality { get; set; } // "locality": "Montevideo",
        public string PostalCode { get; set; } // "postalCode": "11800"
        public double Latitude { get; set; } // coordinates": [-34.88862,
        public double Longitude { get; set; } // -56.17299 ]
    }
}