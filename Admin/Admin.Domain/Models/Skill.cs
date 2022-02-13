

namespace Admin.Domain.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string Name { get; set; }
        public bool IsTechnical { get; set; }
        public int Proficiency { get; set; }
    }
}
