using System;
using System.ComponentModel.DataAnnotations;
using NHibernate.Mapping;

namespace Gourmet.Models
{
    public class Dish
    {
        [Key]
        [Editable(false)]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength=6, ErrorMessage="* Введите название блюда (6-50 символов)")]
        [Display(Name = "Название")]
        public virtual string Name { get; set; }

        [Required]
        [Range(10, 10000, ErrorMessage="* Введите стоимость блюда (10-10000р)")]
        [Display(Name = "Стоимость, р")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")] 
        public virtual int Cost { get; set; }

        [Required]
        [Range(10, 10000, ErrorMessage = "* Введите вес блюда (10-10000гр)")]
        [Display(Name = "Вес, гр")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")] 
        public virtual int Weight { get; set; }

        [Display(Name = "Вегетарианское блюдо")]
        public virtual bool VeganFriendly { get; set; }
    }
}