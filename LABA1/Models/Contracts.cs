using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LABA1
{
    public partial class Contracts
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Номер арендатора")]

        public int RenterId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Гос номер автомобиля")]
        public int CarId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Количество дней")]
        public int DayNumber { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Цена за день")]
        public decimal DayPrice { get; set; }

        public virtual Cars Car { get; set; }
        public virtual Renters Renter { get; set; }
    }
}
