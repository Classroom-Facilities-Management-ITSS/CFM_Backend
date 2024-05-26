﻿namespace ClassroomManagerAPI.Models.Classroom
{
	public class AddClassroomModel
	{
        public string Address { get; set; }
        public DateTime? LastUsed { get; set; }
        public int FacilityAmount { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; }
    }
}