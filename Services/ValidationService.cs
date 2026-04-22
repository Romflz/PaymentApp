using System.Numerics;
using System.Text.RegularExpressions;

namespace PaymentApp.Services;

public class ValidationService : IValidationService
{
    // Valid country codes and their IBAN lengths
    private static readonly Dictionary<string, int> CountryCodes = new()
    {
        { "MT", 31 }, { "DE", 22 }, { "FR", 27 }, { "GB", 22 },
        { "IT", 27 }, { "ES", 24 }, { "NL", 18 }, { "BE", 16 },
        { "AT", 20 }, { "PT", 25 }
    };

    public bool IsValidName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }

    public bool IsValidAmount(decimal amount)
    {
        return amount > 0;
    }

    public bool IsValidIban(string iban)
    {
        return GetIbanError(iban) == null;
    }

    public string? GetIbanError(string iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
            return "IBAN is required.";

        // Remove spaces and convert to uppercase
        iban = iban.Replace(" ", "").ToUpper();

        // Only alphanumeric characters allowed
        if (!Regex.IsMatch(iban, @"^[A-Z0-9]+$"))
            return "IBAN must contain only letters and numbers.";

        // Check country code (first 2 characters)
        var countryCode = iban[..2];
        if (!CountryCodes.ContainsKey(countryCode))
            return $"Invalid country code: {countryCode}";

        // Check length (assignment says 22, but we check per country)
        if (iban.Length != CountryCodes[countryCode])
            return $"IBAN for {countryCode} must be {CountryCodes[countryCode]} characters. Got {iban.Length}.";

        // Mod 97 checksum validation
        if (!PassesMod97Check(iban))
            return "IBAN checksum is invalid.";

        return null; // No error = valid
    }

    private bool PassesMod97Check(string iban)
    {
        // Move first 4 characters to end
        var rearranged = iban[4..] + iban[..4];

        // Convert letters to numbers (A=10, B=11, ..., Z=35)
        var numericString = "";
        foreach (var c in rearranged)
        {
            if (char.IsLetter(c))
                numericString += (c - 'A' + 10).ToString();
            else
                numericString += c;
        }

        // Mod 97 check
        var number = BigInteger.Parse(numericString);
        return number % 97 == 1;
    }
}