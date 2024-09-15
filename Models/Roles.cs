using System.Collections.Generic;

namespace BowlingAlley.Models
{
    public class Roles
    {
        public Roles()
        {
            Reservations = new HashSet<Reservations>();
        }

        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public bool RoleType { get; set; }

        public ICollection<Reservations> Reservations { get; set; }
    }
}
