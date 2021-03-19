using PlanetOfApes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PlanetOfApes
{
    /// <summary>
    /// This is an in-memory ephemral DB Implementation as our current needs are quite simple.
    /// </summary>
    internal sealed class InMemoryFamilyDatabase : IFamilyDatabase
    {
        private class Root
        {
            public string king { get; set; }
            public string queen { get; set; }
            public Child[] children { get; set; }
        }

        private class Child
        {
            public string name { get; set; }
            public bool isMale { get; set; }
            public string spouse { get; set; }
            public Child[] children { get; set; }
        }

        /// <summary>
        /// A dictionary can be used, since a family members name is unique across a family tree.
        /// </summary>
        private Dictionary<string, FamilyMember> _hashMapFamilyMembers = new Dictionary<string, FamilyMember>();

        public InMemoryFamilyDatabase()
        {
            //Load the Json file and populate the initial version of family tree.

            Root rootNode = JsonSerializer.Deserialize<Root>(File.ReadAllText("FamilyTree.json", Encoding.UTF8));

            FamilyMember familyHeadNodeMale = new FamilyMember(rootNode.king, false);

            FamilyMember familyHeadNodeFemale = new FamilyMember(rootNode.queen, true);

            _hashMapFamilyMembers[rootNode.king] = familyHeadNodeMale;

            _hashMapFamilyMembers[rootNode.queen] = familyHeadNodeFemale;

            familyHeadNodeMale.Spouse = familyHeadNodeFemale;

            familyHeadNodeFemale.Spouse = familyHeadNodeMale;

            if (rootNode.children != null && rootNode.children.Length > 0)
            {
                ProcessChildNode(familyHeadNodeMale, familyHeadNodeFemale, rootNode.children);
            }
        }

        /// <summary>
        /// Recursive function to populate the entire family tree.
        /// </summary>
        /// <param name="father"></param>
        /// <param name="mother"></param>
        /// <param name="children"></param>
        private void ProcessChildNode(FamilyMember father, FamilyMember mother, IEnumerable<Child> children)
        {
            List<FamilyMember> childrenToBeAdded = new List<FamilyMember>();
            
            foreach (var child in children)
            {
                FamilyMember childMember = new FamilyMember(child.name, child.isMale);

                FamilyMember spouseNode = null;

                if (!string.IsNullOrEmpty(child.spouse))
                {
                    spouseNode = new FamilyMember(child.spouse, !child.isMale);

                    spouseNode.Spouse = childMember;

                    childMember.Spouse = spouseNode;
                }

                if (child.children != null && child.children.Length > 0)
                {
                    //We can be rest assured here that spouse node will not be null, as children node is present.
                    FamilyMember fatherTemp = childMember.IsMale ? childMember : spouseNode;

                    ProcessChildNode(fatherTemp, fatherTemp.Spouse, child.children);
                }

                childMember.Father = father;

                childMember.Mother = mother;

                _hashMapFamilyMembers[childMember.Name] = childMember;

                if (childMember.Spouse != null)
                {
                    _hashMapFamilyMembers[childMember.Spouse.Name] = childMember.Spouse;
                }

                childrenToBeAdded.Add(childMember);
            }

            //Both mother and father nodes will share a reference to same list in memory
            father.Children = mother.Children = childrenToBeAdded;
        }


        public void AddChild(string mothersName, string childName, bool isChildMale)
        {
            FamilyMember motherNodeLocated;

            if (!_hashMapFamilyMembers.TryGetValue(mothersName, out motherNodeLocated))
            {
                throw new InvalidOperationException("Unable to find the base member node with name -" + mothersName);
            }

            if (motherNodeLocated.IsMale)
            {
                throw new InvalidOperationException("Cannot add children under the male member - " + mothersName);
            }

            if (motherNodeLocated.Spouse == null)
            {
                throw new InvalidOperationException(mothersName + " is not married, and hence unable to add children without spouse node present.");
            }

            FamilyMember childNode = new FamilyMember(childName, isChildMale);

            childNode.Mother = motherNodeLocated;

            childNode.Father = motherNodeLocated.Spouse;

            //Mother and father share same children collection with InMemory reference, hence no need to update the Father node.
            motherNodeLocated.Children.Add(childNode);

            _hashMapFamilyMembers[childNode.Name] = childNode;
        }

        public FamilyMember GetFamilyMember(string memberName)
        {
            FamilyMember searchMember = null;

            _hashMapFamilyMembers.TryGetValue(memberName, out searchMember);

            return searchMember;
        }
    }


}
