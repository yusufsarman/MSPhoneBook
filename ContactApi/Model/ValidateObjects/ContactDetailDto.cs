﻿using ContactApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactApi.Model.ValidateObjects
{
    public class ContactDetailDto : BaseDto
    {

         public Guid? ContactId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [EnumDataType(typeof(ContactDetailTypeEnum), ErrorMessage = "Invalid ContactDetailType.")]
        public ContactDetailTypeEnum ContactDetailType { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        [StringLength(50, ErrorMessage = "Content cannot exceed 250 characters.")]
        public string Content { get; set; }
    }
}
