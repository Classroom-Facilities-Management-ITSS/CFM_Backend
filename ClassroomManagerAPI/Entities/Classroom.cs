﻿using ClassroomManagerAPI.Enums;
using ClassroomManagerAPI.Enums.ErrorCodes;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Entities
{
    public class Classroom : BaseEntity
    {
        [Required(ErrorMessage = nameof(ErrorSystemEnum.Required))]
        public string? Address { get; set; }
        public DateTime LastUsed { get; set; }
        public int FacilityAmount { get; set; }
        public int MaxSize { get; set; }
        public string? Note { get; set; }
        public ClassroomStatusEnum Status { get; set; }
        public ICollection<Facility>? Facilities { get; set; }
        public ICollection<Report>? Reports { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
    }
}
