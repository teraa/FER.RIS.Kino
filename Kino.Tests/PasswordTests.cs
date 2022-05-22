using Kino.Services;
using Xunit;

namespace Kino.Tests;

public class PasswordTests
{
    private readonly PasswordService _passwordService;
    
    public PasswordTests()
    {
        _passwordService = new PasswordService();
    }
    
    [Fact]
    public void SameInput_Tests_True()
    {
        const string password = "pw";
        var (hash, salt) = _passwordService.Hash(password);

        bool result = _passwordService.Test(password, hash, salt);
        
        Assert.True(result);
    }
    
    [Fact]
    public void DifferentPassword_Tests_False()
    {
        var (hash, salt) = _passwordService.Hash("pw");

        bool result = _passwordService.Test("p?", hash, salt);
        
        Assert.False(result);
    }

    [Fact]
    public void DifferentHash_Tests_False()
    {
        const string password = "pw";
        var (hash, salt) = _passwordService.Hash(password);

        hash[0] ^= 1;
        bool result = _passwordService.Test(password, hash, salt);
        
        Assert.False(result);
    }

    [Fact]
    public void DifferentSalt_Tests_False()
    {
        const string password = "pw";
        var (hash, salt) = _passwordService.Hash(password);

        salt[0] ^= 1;
        bool result = _passwordService.Test(password, hash, salt);
        Assert.False(result);
    }
}