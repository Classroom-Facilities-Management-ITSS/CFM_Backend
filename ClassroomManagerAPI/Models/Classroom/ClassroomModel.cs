﻿using ClassroomManagerAPI.Models.Facility;
using ClassroomManagerAPI.Models.Report;

namespace ClassroomManagerAPI.Models.Classroom
{
	public class ClassroomModel : AddClassroomModel
	{
		public Guid? Id { get; set; }
        public int? FacilityAmount { get; set; }
	}
}
