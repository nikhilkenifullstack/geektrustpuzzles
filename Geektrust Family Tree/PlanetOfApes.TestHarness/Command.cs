using System;
using System.Linq;

namespace PlanetOfApes.TestHarness
{
    abstract class Command
    {
        public abstract void Execute(IFamilyTree familyTree);
    }

    class AddCommand : Command
    {
        public string MothersName { get; set; }

        public string ChildName { get; set; }

        public bool IsChildMale { get; set; }

        public override void Execute(IFamilyTree familyTree)
        {
            try
            {
                familyTree.AddChild(MothersName, ChildName, IsChildMale);

                Console.WriteLine("Operation executed successfully.. Added  the child - " + ChildName + " under the mother - " + MothersName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }

    class GetCommand : Command
    {
        public string MemberName { get; set; }

        public string RelationShipToGet { get; set; }

        public override void Execute(IFamilyTree familyTree)
        {
            try
            {
                string relationsObtained = string.Join(" , ", familyTree.GetFamilyMembersForRelation(MemberName, RelationShipToGet).ToArray());

                Console.WriteLine("Found the following relations - " + relationsObtained + " for " + this.MemberName + " for relation type -" + RelationShipToGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
