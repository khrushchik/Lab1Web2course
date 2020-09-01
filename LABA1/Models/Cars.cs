using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LABA1
{
    public partial class Cars
    {
        public Cars()
        {
            Contracts = new HashSet<Contracts>();
        }

        public int Id { get; set; }
        [Display(Name = "Марка")]
        public int LabelId { get; set; }
        [Display(Name = "Кузов")]
        public int BodyId { get; set; }
        [Display(Name = "Цвет")]
        public int ColorId { get; set; }
        [Display(Name = "Год")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public int Year { get; set; }
        [Display(Name = "Коробка передач")]
        public int TransmissionId { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Display(Name = "Арендодатель")]
        public int LandlordId { get; set; }
        [Display(Name = "Гос номер")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        public string GovNumber { get; set; }//new

        public virtual Bodies Body { get; set; }
        public virtual Colors Color { get; set; }
        public virtual Labels Label { get; set; }
        public virtual Landlords Landlord { get; set; }
        public virtual Transmissions Transmission { get; set; }
        public virtual ICollection<Contracts> Contracts { get; set; }
    }
}
