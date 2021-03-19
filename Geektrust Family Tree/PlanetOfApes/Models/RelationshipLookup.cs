using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetOfApes.Models
{
    /// <summary>
    /// An interface to offload relationship lookup. In case you need to use some intelligent system with TP libraries down the line.
    /// </summary>
    interface IRelationshipLookup
    {
        IEnumerable<FamilyMember> FindRelations(FamilyMember memberInCurrentContext, eRelationType relation);
    }

    class RelationshipLookup : IRelationshipLookup
    {
        public IEnumerable<FamilyMember> FindRelations(FamilyMember memberInCurrentContext, eRelationType relationType)
        {        
            if (memberInCurrentContext == null)
            {
                throw new ArgumentNullException("memberInCurrentContext");
            }

            switch (relationType)
            {
                case eRelationType.Son:
                    return memberInCurrentContext.Children.Where(a => a.IsMale);
                case eRelationType.Daughter:
                    return memberInCurrentContext.Children.Where(a => !a.IsMale);
                case eRelationType.Father:
                    if (memberInCurrentContext.Father == null)
                    {
                        throw new InvalidOperationException("Reached the zeroth node. Cannot traverse any further");
                    }
                    return new FamilyMember[] { memberInCurrentContext.Father };
                case eRelationType.Mother:
                    if (memberInCurrentContext.Mother == null)
                    {
                        throw new InvalidOperationException("Reached the zeroth node. Cannot traverse any further");
                    }
                    return new FamilyMember[] { memberInCurrentContext.Mother };
                case eRelationType.Sibling:
                    return GetSiblings(memberInCurrentContext, null);
                case eRelationType.PaternalUncle:
                    return GetPaternalUncles(memberInCurrentContext);
                case eRelationType.MaternalUncle:
                    return GetMaternalUncles(memberInCurrentContext);
                case eRelationType.PaternalAunt:
                    return GetPaternalAunts(memberInCurrentContext);
                case eRelationType.MaternalAunt:
                    return GetMaternalAunts(memberInCurrentContext);
                case eRelationType.SisterInLaw:
                    return GetSisterInLaws(memberInCurrentContext);
                case eRelationType.BrotherInLaw:
                    return GetBrotherInLaws(memberInCurrentContext);
                default:
                    throw new InvalidOperationException(relationType + "- This relationship type is not supported yet.");
            }
        }

        private IEnumerable<FamilyMember> GetSiblings(FamilyMember memberInCurrentContext, bool? isMaleFilter)
        {
            if (memberInCurrentContext.Father == null)
            {
                //We have mostly reached node 0 - and hence better to return an empty list only. No need to throw exceptions.
                return new FamilyMember[] { };
            }
            IEnumerable<FamilyMember> searchSet = memberInCurrentContext.Father.Children.Where(a => a.Name != memberInCurrentContext.Name);

            if (isMaleFilter.HasValue)
            {
                searchSet = searchSet.Where(a => a.IsMale == isMaleFilter.Value);
            }

            return searchSet;
        }

        private IEnumerable<FamilyMember> GetSisterInLaws(FamilyMember memberInCurrentContext)
        {
            List<FamilyMember> foundMembers = new List<FamilyMember>();

            if (memberInCurrentContext.Spouse != null)
            {
                //Get all spouses sisters
                foundMembers.AddRange(GetSiblings(memberInCurrentContext, false));
            }

            //Get all brothers.
            var brothers = GetSiblings(memberInCurrentContext, true);

            foreach (var brother in brothers)
            {
                if (brother.Spouse != null)
                {
                    foundMembers.Add(brother.Spouse);
                }
            }

            return foundMembers;
        }

        private IEnumerable<FamilyMember> GetBrotherInLaws(FamilyMember memberInCurrentContext)
        {
            List<FamilyMember> foundMembers = new List<FamilyMember>();

            //Get all sisters.
            var sisters = GetSiblings(memberInCurrentContext, false);

            foreach (var sister in sisters)
            {
                if (sister.Spouse != null)
                {
                    foundMembers.Add(sister.Spouse);
                }
            }

            if (memberInCurrentContext.Spouse != null)
            {
                //Get all spouses brothers
                foundMembers.AddRange(GetSiblings(memberInCurrentContext, true));
            }

            return foundMembers;
        }

        private IEnumerable<FamilyMember> GetPaternalUncles(FamilyMember memberInCurrentContext)
        {
            if (memberInCurrentContext.Father == null)
            {
                throw new InvalidOperationException("Reached the zeroth node. Cannot traverse any further");
            }
            //A grandfather node is a must for this.
            if (memberInCurrentContext.Father.Father != null)
            {
                return memberInCurrentContext.Father.Father.Children.Where(a => a.IsMale && a.Name != memberInCurrentContext.Father.Name);
            }
            else
            {
                return new FamilyMember[] { };
            }
        }

        private IEnumerable<FamilyMember> GetMaternalUncles(FamilyMember memberInCurrentContext)
        {
            if (memberInCurrentContext.Mother == null)
            {
                throw new InvalidOperationException("Reached the zeroth node. Cannot traverse any further");
            }
            //A grandfather node is a must for this.
            if (memberInCurrentContext.Mother.Father != null)
            {
                return memberInCurrentContext.Mother.Father.Children.Where(a => !a.IsMale);
            }
            else
            {
                return new FamilyMember[] { };
            }
        }

        private IEnumerable<FamilyMember> GetPaternalAunts(FamilyMember memberInCurrentContext)
        {
            if (memberInCurrentContext.Father == null)
            {
                throw new InvalidOperationException("Reached the zeroth node. Cannot traverse any further");
            }
            //A grandfather node is a must for this.
            if (memberInCurrentContext.Father.Father != null)
            {
                return memberInCurrentContext.Father.Father.Children.Where(a => !a.IsMale);
            }
            else
            {
                return new FamilyMember[] { };
            }
        }

        private IEnumerable<FamilyMember> GetMaternalAunts(FamilyMember memberInCurrentContext)
        {
            if (memberInCurrentContext.Father == null)
            {
                throw new InvalidOperationException("Reached the zeroth node. Cannot traverse any further");
            }
            //A grandfather node is a must for this.
            if (memberInCurrentContext.Mother.Father != null)
            {
                return memberInCurrentContext.Mother.Father.Children.Where(a => !a.IsMale && a.Name != memberInCurrentContext.Mother.Name);
            }
            else
            {
                return new FamilyMember[] { };
            }
        }
    }
}
