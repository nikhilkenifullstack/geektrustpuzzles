namespace PlanetOfApes.Models
{
    /// <summary>
    /// These are the supported Relation types so far.
    /// In case more are needed add an entry here as well.
    /// </summary>
    enum eRelationType
    {
        Son,
        Daughter,
        Father,
        Mother,
        Sibling,
        PaternalUncle,
        MaternalUncle,
        PaternalAunt,
        MaternalAunt,
        SisterInLaw,
        BrotherInLaw
    }
}
