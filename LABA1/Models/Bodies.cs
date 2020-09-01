using System;
using System.Collections.Generic;

namespace LABA1
{
    public partial class Bodies
    {
        public Bodies()
        {
            Cars = new HashSet<Cars>();
        }

        public int BodyId { get; set; }
        public string Body { get; set; }

        public virtual ICollection<Cars> Cars { get; set; }
    }
}
