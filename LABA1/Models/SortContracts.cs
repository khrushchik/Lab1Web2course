using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LABA1.Models
{
    public enum SortContracts
    {
        RenterAsc,
        RenterDesc,
        CarAsc,
        CarDesc,
        StartDateAsc,
        StartDateDesc,
        DayNumberAsc,
        DayNumberDesc,
        DayPriceAsc, // по возрастанию
        DayPriceDesc // по убыванию
    }
}
