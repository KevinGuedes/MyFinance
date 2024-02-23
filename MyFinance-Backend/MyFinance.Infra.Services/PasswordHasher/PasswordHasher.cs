﻿using BC = BCrypt.Net.BCrypt;

namespace MyFinance.Services.PasswordHasher;

public sealed class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string plainTextPassword)
        => BC.EnhancedHashPassword(plainTextPassword, 16);

    public bool VerifyPassword(string plainTextPassword, string passwordHash)
        => BC.EnhancedVerify(plainTextPassword, passwordHash);
}