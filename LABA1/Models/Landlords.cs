using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LABA1
{
    public partial class Landlords
    {
        public Landlords()
        {
            Cars = new HashSet<Cars>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Страна")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Контактное лицо")]
        public string ContartPerson { get; set; }
        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Phone { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<Cars> Cars { get; set; }
    }
}
