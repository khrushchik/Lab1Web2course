using System;
using System.Collections.Generic;

namespace LABA1
{
    public partial class Countries
    {
        public Countries()
        {
            Landlords = new HashSet<Landlords>();
        }

        public int CountryId { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Landlords> Landlords { get; set; }
    }
}
