using Blackbird.Applications.Sdk.Common;

namespace Apps.Memoq.Models.Users.Requests;

public class CreateUserRequest
{
    [Display("User name")]
    public string UserName { get; set; }
    
    [Display("Address")]
    public string? Address { get; set; }

    [Display("Email address")]
    public string? EmailAddress { get; set; }

    [Display("Full name")]
    public string? FullName { get; set; }

    [Display("Is disabled")]
    public bool? IsDisabled { get; set; }

    [Display("Is subvendor manager")]
    public bool? IsSubvendorManager { get; set; }

    [Display("LT email address")]
    public string? LTEmailAddress { get; set; }

    [Display("LT full name")]
    public string? LTFullName { get; set; }

    [Display("LT user name")]
    public string? LTUsername { get; set; }

    [Display("Language pairs")]
    public string? LanguagePairs { get; set; }

    [Display("Mobile phone number")]
    public string? MobilePhoneNumber { get; set; }

    [Display("Password")]
    public string? Password { get; set; }

    [Display("Phone number")]
    public string? PhoneNumber { get; set; }

    [Display("Plain text password")]
    public string? PlainTextPassword { get; set; }

    [Display("Secondary SID")]
    public string? SecondarySID { get; set; }
}