using System;
using System.Collections.Generic;

namespace LABA1
{
    public partial class Colors
    {
        public Colors()
        {
            Cars = new HashSet<Cars>();
        }

        public int ColorId { get; set; }
        public string Color { get; set; }

        public virtual ICollection<Cars> Cars { get; set; }
    }
}
