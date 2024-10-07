using CompaneyMvcDAL.Modules;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace CompaneyMvcPL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Max length is 50")]
        [MinLength(5, ErrorMessage = "min length is 5")]
        public string Name { get; set; }
        [Range(22, 35, ErrorMessage = "age will be between 22,35")]
        public int Age { get; set; }
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "address will be like 123-street-city-country")]
        public string Address { get; set; }
        public bool IsActive { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [EmailAddress]
        public string E_mail { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public DateTime HiringDate { get; set; }
       
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
