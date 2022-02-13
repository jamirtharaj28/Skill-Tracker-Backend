using SkillTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profile.Domain.Entities
{
    public class ProfileEntity : EntityBase
    {
        public string EmpId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string UserId { get; set; }

        public DateTime EventTime { get; set; }

        public List<Skill> skills { get; set; }
    }
}
