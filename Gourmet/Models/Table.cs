using System;
using System.ComponentModel.DataAnnotations;
using NHibernate.Mapping;

namespace Gourmet.Models
{
    public class Table
    {
        public Table()
        {
            this.IsFree = true;
        }

        [Key]
        [Editable(false)]
        [Display(Name = "№ стола")]
        public virtual int Id { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "* Введите вместимость стола")]
        [Display(Name = "Вместимость стола, чел.")]
        public virtual string Capacity { get; set; }

        [Display(Name = "Стол свободен")]
        public virtual bool IsFree { get; set; }

        // ID текущего заказа, изменять при создании/завершении заказов
        public virtual int CurrentOrder { get; set; }
    }
}