using FixMyCampus.Domain.Entities;

namespace FixMyCampus.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
