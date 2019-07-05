using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vid_p1_ati_ii.Models
{
    public class Student
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement]
        [Display(Name = "Student ID")]
        [Required(ErrorMessage ="Field required")]
        public int StudentID { get; set; }
        [BsonElement]
        [Required]
        [RegularExpression("^[a-zA-z ]*$",ErrorMessage = "Only Alphabet")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [BsonElement]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [BsonElement]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}
