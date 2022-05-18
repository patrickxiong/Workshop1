using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Contacts
    {
        public string Contact_GUID { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Work_Tel { get; set; }
        public string Mobile_Tel { get; set; }
        public int Department_ID { get; set; }
        public virtual Departments Department { get; set; }
    }
}
