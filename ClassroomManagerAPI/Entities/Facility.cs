﻿using ClassroomManagerAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClassroomManagerAPI.Entities
{
    public class Facility : BaseEntity
    {
        [Required(ErrorMessage = nameof(ErrorSystemEnum.Required))]
        public string Name { get; set; }
        [Required(ErrorMessage = nameof(ErrorSystemEnum.Required))]
        public int Count { get; set; }
        public FacilityStatusEnum Status { get; set; }
        public string Version { get; set; }
        public string? Note { get; set; }
        public Guid? ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }

    }
}
