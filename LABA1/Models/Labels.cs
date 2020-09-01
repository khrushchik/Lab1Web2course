using System;
using System.Collections.Generic;

namespace LABA1
{
    public partial class Labels
    {
        public Labels()
        {
            Cars = new HashSet<Cars>();
        }

        public int LableId { get; set; }
        public string Lable { get; set; }

        public virtual ICollection<Cars> Cars { get; set; }
    }
}
