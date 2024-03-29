﻿using Core.Models.Ids;

namespace Core.Models.User;

public sealed record UserInformation
{
    public required UserId UserId { get; init; }
    public required UserBalance Balance { get; init; }
}