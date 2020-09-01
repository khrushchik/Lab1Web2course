using System;
using System.Collections.Generic;

namespace LABA1
{
    public partial class Transmissions
    {
        public Transmissions()
        {
            Cars = new HashSet<Cars>();
        }

        public int TransmissionId { get; set; }
        public string Trasmission { get; set; }

        public virtual ICollection<Cars> Cars { get; set; }
    }
}
