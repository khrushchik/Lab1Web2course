using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LABA1
{
    public partial class Renters
    {
        public Renters()
        {
            Contracts = new HashSet<Contracts>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Паспорт")]
        public string Passport { get; set; }
        [Display(Name = "телефон")]

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Phone { get; set; }
        [Display(Name = "Стаж вождения")]

        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string DriveExperience { get; set; }
        [Display(Name = "Адрес")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Address { get; set; }
        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string Name { get; set; }


        public virtual ICollection<Contracts> Contracts { get; set; }
    }
}
