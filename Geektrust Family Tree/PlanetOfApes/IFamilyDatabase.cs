using PlanetOfApes.Models;

namespace PlanetOfApes
{
    /// <summary>
    /// Abstracting the DB layer for a SOLID principle alignment.
    /// This will allow you to use any DB you want in future, MySQL, Cassandra and what not... Go crazy man!!
    /// </summary>
    internal interface IFamilyDatabase
    {
        /// <summary>
        /// This method allows you to add a descendant child node below a female node only.
        /// In case you attempt to add a child below a malde node, or try to add child to a female without a spouse, this will throw an InvalidOperation exception.
        /// </summary>
        /// <param name="mothersName"></param>
        /// <param name="childName"></param>
        /// <param name="isChildMale"></param>
        void AddChild(string mothersName, string childName, bool isChildMale);

        /// <summary>
        /// This will return a Family member in case its found with the unique member name attribute. If not, it will return null.
        /// </summary>
        /// <param name="memberName"></param>
        /// <returns></returns>
        FamilyMember GetFamilyMember(string memberName);
    }


}
