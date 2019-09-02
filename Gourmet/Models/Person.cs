using System;
using System.ComponentModel.DataAnnotations;
using NHibernate.Mapping;

namespace Gourmet.Models
{
    public class Person
    {
        [Key]
        [Editable(false)]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "* Введите ФИО сотрудника")]
        [Display(Name = "ФИО сотрудника")]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "* Выберите должность сотрудника")]
        [Display(Name = "Должность")]
        public virtual string Position { get; set; }
    }
}