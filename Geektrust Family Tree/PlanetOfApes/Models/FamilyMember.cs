using System.Collections.Generic;

namespace PlanetOfApes.Models
{
    /// <summary>
    /// This represents one single person in the Family Tree. Even the King and Queen are represented with same node.
    /// </summary>
    class FamilyMember
    {
        public FamilyMember(string name, bool isMale)
        {
            Name = name;
            IsMale = isMale;
            Children = new List<FamilyMember>();
        }

        public FamilyMember Father { get; set; }

        public FamilyMember Mother { get; set; }

        public bool IsMale { get; }

        public FamilyMember Spouse { get; set; }

        public IList<FamilyMember> Children { get; set; }

        public string Name { get; }
    }
}
