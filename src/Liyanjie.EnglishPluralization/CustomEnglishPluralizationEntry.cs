using Liyanjie.EnglishPluralization.Internals;

namespace Liyanjie.EnglishPluralization;

/// <summary>
/// Represents a custom pluralization term to be used by the <see cref="T:System.Data.Entity.Infrastructure.Pluralization.EnglishPluralizationService" />
/// </summary>
public class CustomEnglishPluralizationEntry
{
    /// <summary>
    /// Get the singular.
    /// </summary>
    public string Singular
    {
        get;
        private set;
    }

    /// <summary>
    /// Get the plural.
    /// </summary>
    public string Plural
    {
        get;
        private set;
    }

    /// <summary>
    /// Create a new instance
    /// </summary>
    /// <param name="singular">A non null or empty string representing the singular.</param>
    /// <param name="plural">A non null or empty string representing the plural.</param>
    public CustomEnglishPluralizationEntry(string singular, string plural)
    {
        Check.NotEmpty(singular, "singular");
        Check.NotEmpty(plural, "plural");
        this.Singular = singular;
        this.Plural = plural;
    }
}
