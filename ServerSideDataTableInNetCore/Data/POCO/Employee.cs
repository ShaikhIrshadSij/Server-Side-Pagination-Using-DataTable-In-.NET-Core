using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSideDataTableInNetCore.Data.POCO
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public string Street { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
    }
}
