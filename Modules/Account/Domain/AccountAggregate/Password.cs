using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Modules.Account.Domain.AccountAggregate.Exceptions;
using Modules.Account.Domain.Shared.Consts;
using System.Security.Cryptography;
using System.Text;

namespace Modules.Account.Domain.AccountAggregate;
public class Password : ValueObject
{
    private static readonly string[] _specialCharacters = ["!", "@", "#", "$", "%", "&", "*", "(", ")", "-", "_", "+", "=", "{", "}", "[", "]", ":", ";", "<", ">", ",", ".", "?", "/", "|", "\\"];
    private static readonly string[] _numbers = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];
    private static readonly string[] _upperCaseLetters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
    private static readonly string[] _lowerCaseLetters = ["a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"];
    private static string[] _allCharacters = [.. _specialCharacters, .. _numbers, .. _upperCaseLetters, .. _lowerCaseLetters];


    public string HashedValue { get; private set; }
    public string ClearValue { get; private set; }
    public Salt Salt { get; private set; }

    private Password()
    {
    }

    public Password(string clearPassword, Salt salt, IHashProvider hashProvider)
    {
        Guard.Against.NullOrWhiteSpace(clearPassword, nameof(clearPassword), "Şifre boş olamaz!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Length >= PasswordConsts.MinPasswordLength, "Şifre en az altı karakter uzunluğunda olmalıdır!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Any(pc => !char.IsLetterOrDigit(pc)), "Şifre en az bir adet özel karakter içermelidir!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Any(char.IsDigit), "Şifre en az bir adet rakam içermelidir!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Any(char.IsUpper), "Şifre en az bir adet büyük harf içermelidir!");
        Guard.Against.InvalidInput(clearPassword, nameof(clearPassword), x => x.Any(char.IsLower), "Şifre en az bir adet küçük harf içermelidir!");

        Guard.Against.Null(salt, nameof(salt), "Salt parametresi null olamaz!");

        ClearValue = clearPassword;
        Salt = salt;
        HashedValue = hashProvider.HashData(clearPassword, salt.Value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return HashedValue;
        yield return Salt;
    }

    public void Change(string currentClearPassword, string newClearPassword, string newClearPasswordConfirmation, string username, IHashProvider hashProvider)
    {
        Guard.Against.NullOrWhiteSpace(currentClearPassword, exceptionCreator: () => new CurrentPasswordIsEmptyException());
        Guard.Against.NullOrWhiteSpace(newClearPassword, exceptionCreator: () => new NewPasswordIsEmptyException());
        Guard.Against.InvalidInput(newClearPassword, nameof(newClearPassword), x => x != currentClearPassword, exceptionCreator: () => new SamePasswordException());
        Guard.Against.InvalidInput(newClearPassword, nameof(newClearPassword), x => x != username, "Kullanıcı Adı ve Şifre birbirine eşit olamaz!");
        Guard.Against.InvalidInput(newClearPassword, nameof(newClearPassword), x => x == newClearPasswordConfirmation, exceptionCreator: () => new PasswordConfirmationMismatchException());
        Guard.Against.InvalidInput(this, nameof(currentClearPassword), x => x == new Password(currentClearPassword, Salt, hashProvider), exceptionCreator: () => new CurrentPasswordIsWrongException());

        var newPassword = new Password(newClearPassword, Salt.Generate(), hashProvider);
        ClearValue = newPassword.ClearValue;
        Salt = newPassword.Salt;
        HashedValue = newPassword.HashedValue;
    }

    public static Password Generate(IHashProvider hashProvider, int length = PasswordConsts.MinPasswordLength)
    {
        var clearPassword = new StringBuilder();

        clearPassword.Append(_upperCaseLetters[RandomNumberGenerator.GetInt32(_upperCaseLetters.Length)]);
        clearPassword.Append(_specialCharacters[RandomNumberGenerator.GetInt32(_specialCharacters.Length)]);
        clearPassword.Append(_numbers[RandomNumberGenerator.GetInt32(_numbers.Length)]);
        clearPassword.Append(_lowerCaseLetters[RandomNumberGenerator.GetInt32(_lowerCaseLetters.Length)]);

        RandomNumberGenerator.Shuffle<string>(_allCharacters);

        for (int i = 4; i < length; i++)
        {
            clearPassword.Append(_allCharacters[RandomNumberGenerator.GetInt32(_allCharacters.Length)]);
        }

        return new Password(clearPassword.ToString(), Salt.Generate(), hashProvider);
    }
}
