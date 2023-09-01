﻿using ReportApi.Entities;

namespace ReportApi.Model.ValidateObjects
{
    public class ReportDetailDto:BaseDto
    {
        public Guid ReportId { get; set; }       
        public string Location { get; set; }
        public int PhoneCount { get; set; }
        public int EmailCount { get; set; }
    }
}
