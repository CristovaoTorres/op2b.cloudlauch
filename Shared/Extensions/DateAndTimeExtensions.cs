using Platform.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Extensions
{
    public static class DateAndTimeExtensions
    {

        public static eDayOfWeekEF[] GetAllWeekDays()
        {
            return new eDayOfWeekEF[] { eDayOfWeekEF.Monday, eDayOfWeekEF.Tuesday, eDayOfWeekEF.Wednesday, eDayOfWeekEF.Thursday, eDayOfWeekEF.Friday, eDayOfWeekEF.Saturday, eDayOfWeekEF.Sunday};

        }

        public static string ToPortugueseBrazil(this eDayOfWeekEF dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case eDayOfWeekEF.Sunday:
                    return "Domingo";
                case eDayOfWeekEF.Monday:
                    return "Segunda-feira";
                case eDayOfWeekEF.Tuesday:
                    return "Terça-feira";
                case eDayOfWeekEF.Wednesday:
                    return "Quarta-feira";
                case eDayOfWeekEF.Thursday:
                    return "Quinta-feira";
                case eDayOfWeekEF.Friday:
                    return "Sexta-feira";
                case eDayOfWeekEF.Saturday:
                    return "Sábado";
                default:
                    return "desconhecido";
            }

        }



    }
}
