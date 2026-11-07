namespace DonationHub.API.Contract.Authentication
{
    public sealed record CreateStaffUserRequest(
        string Email,
        string FullName,
        string Password,
        string PreferredLanguage);
}
