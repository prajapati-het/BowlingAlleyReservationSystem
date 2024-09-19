using System.Collections.Generic;

namespace BowlingAlley.Models
{
    public class Roles
    {
        public Roles()
        {
            Reservations = new HashSet<Reservations>();
            ReservationRejections = new HashSet<ReservationRejections>();
        }

        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public bool RoleType { get; set; }
        public string Password { get; set; }

        public ICollection<Reservations> Reservations { get; set; }

        public ICollection<ReservationRejections> ReservationRejections { get; set; }
    }
}
