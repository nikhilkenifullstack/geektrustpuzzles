using PlanetOfApes.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanetOfApes
{
    public sealed class FamilyTree : IFamilyTree
    {
        private IFamilyDatabase _familyDBInstance;

        private IRelationshipLookup _relationLookup;

        public FamilyTree()
        {
            //Initialise the DB layer or service to be consumed.
            //In case you want this can be injected by sevice discovery or Dependency injection pattern too.
            _familyDBInstance = new InMemoryFamilyDatabase();

            _relationLookup = new RelationshipLookup();
        }

        public IEnumerable<string> GetFamilyMembersForRelation(string memberName, string relationType)
        {
            FamilyMember member = _familyDBInstance.GetFamilyMember(memberName);

            if (member == null)
            {
                throw new InvalidOperationException("You attempted to find relations for a non-existent family member");
            }

            if (Enum.TryParse(relationType, out eRelationType relationTypeInternal))
            {
                return _relationLookup.FindRelations(member, relationTypeInternal).Select(a => a.Name).ToList();
            }
            else
            {
                throw new InvalidOperationException("You attempted to find relations which are not supported by the program yet.");
            }
        }

        public void AddChild(string motherName, string childName, bool isMale)
        {
            _familyDBInstance.AddChild(motherName, childName, isMale);
        }

    }
}
