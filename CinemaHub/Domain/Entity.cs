using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaHub.Domain
{
    public abstract class Entity
    {
        protected Entity()
        {
            CreatedOn = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
