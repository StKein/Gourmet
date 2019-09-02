using System;
using System.ComponentModel.DataAnnotations;
using NHibernate.Mapping;

namespace Gourmet.Models
{
    public class Order
    {
        [Key]
        [Editable(false)]
        [Display(Name = "№ заказа")]
        public virtual int Id { get; set; }


        [Required]
        [Range(1, 10000, ErrorMessage = "* Введите номер стола")]
        [Display(Name = "Номер стола")]
        public virtual int Table { get; set; }


        [Required]
        [Range(1, 10000, ErrorMessage = "* Укажите официанта")]
        [Display(Name = "Официант")]
        public virtual int Waiter { get; set; }

        // ФИО сотрудника, для представлений
        public virtual string WaiterName { get; set; }


        [Required]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "* Выберите заказываемые блюда")]
        [Display(Name = "Меню закзаа")]
        public virtual string Menu { get; set; }

        // Детализация выбранных блюд, для представлений
        public virtual System.Collections.Generic.Dictionary<string, int> MenuDescription { get; set; }


        [Display(Name = "Статус заказа")]
        public virtual int Status { get; set; }

        // Описание статуса, для представлений
        public virtual string StatusDescription
        {
            get
            {
                switch (this.Status)
                {
                    case -1:
                        return "Отменен";
                    case 1:
                        return "Выполнен";
                    default:
                        return "В работе";
                }
            }
        }
    }
}