﻿namespace ClassroomManagerAPI.Models.Dto
{
	public class ClassroomDto
	{
		public string ClassNumber { get; set; }
		public string LastUsed { get; set; }
		public int FacilityAmount { get; set; }
		public string? Note { get; set; }
	}
}
