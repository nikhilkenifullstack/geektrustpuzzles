using System.Collections.Generic;

namespace PlanetOfApes
{
    public interface IFamilyTree
    {
        void AddChild(string motherName, string childName, bool isMale);

        IEnumerable<string> GetFamilyMembersForRelation(string memberName, string relationType);
    }
}