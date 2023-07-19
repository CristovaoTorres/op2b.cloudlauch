using Platform.Shared.Extensions;
using Platform.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Shared.Models
{

    /// <summary>
    /// Informacoes  de agendamento para ligar/desligar virtual-machines que podem ser feitas em horarios pre-agendados, pelos usuarios.
    /// </summary>
    [Table("UserVMSchedulings")]
    public class UserVMScheduling
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column("Id")]
        //public int VMSchedulingId { get; set; }


        /// <summary>
        /// Cria um novo Schedule valido para todos os dias da semana.
        /// </summary>
        /// <returns></returns>
        public static UserVMScheduling CreateEmpty(string userId, eVMActionTypes type)
        {
           var inicioDia = new DateTime( 1900, 1, 1, 0, 0, 0);
           var finalDia = new DateTime(1900, 1, 1, 23, 59, 59);

            return new UserVMScheduling 
            { 
                UserId = userId, 
                ActionType = type, 
                StartTime = inicioDia,
                EndTime = finalDia,
                WeekDaySunday = true, 
                WeekDayFriday = true, 
                WeekDayMonday = true, 
                WeekDaySaturday = true,
                WeekDayThursday = true,
                WeekDayTuesday = true, 
                WeekDayWednesday = true
            };
        }




        [Key]
        public string UserId { get; set; }

        [Key]
        [Column("ActionTypeId")]
        public int ActionTypeId
        {
            get
            {
                return (int)ActionType;
            }

            set
            {
                ActionType = (eVMActionTypes)value;
            }
        }



        /// <summary>
        /// Tipo de acao que o usuario pode fazer nas VM.
        /// A acao vai depender de o usuario ter ou nao o "Claim" <see cref="ClaimsConstants.PERMISSION_CLAIM_CAN_START_VM"/> e/ou <see cref="ClaimsConstants.PERMISSION_CLAIM_CAN_STOP_VM"/>
        /// </summary>
        [NotMapped]
        public eVMActionTypes ActionType
        {
            get 
            { 
                return actionType; 
            }
            set
            {
                actionType = value;
            }
        }





        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }




        private DateTime startTime;
        private DateTime endTime;
     
        private eVMActionTypes actionType;

        public bool WeekDaySunday { get; set; }
        public bool WeekDayMonday { get; set; }

        public bool WeekDayTuesday { get; set; }
        public bool WeekDayWednesday { get; set; }
        public bool WeekDayThursday { get; set; }
        public bool WeekDayFriday { get; set; }
        public bool WeekDaySaturday { get; set; }

        /// <summary>
        /// HOra de unicio
        /// </summary>
        public DateTime StartTime { get => startTime; set => startTime = value; }

        /// <summary>
        /// Hora de termino
        /// </summary>
        public DateTime EndTime { get => endTime; set => endTime = value; }


        //[NotMapped]
        //public string EndTimeString
        //{
        //    get
        //    {
        //        return endTime.ToString();
        //    }
        //    set
        //    {
        //        TimeSpan.TryParse(value, out endTime);
        //    }
        //}



        //[NotMapped]
        //public string StartTimeString
        //{
        //    get
        //    {
        //        return StartTime.ToString();
        //    }
        //    set
        //    {
        //        TimeSpan.TryParse(value, out startTime);
        //    }
        //}


    }
}
