﻿using Core.Models.Asks;
using Core.Models.Ids;

namespace Core.Repositories;

public interface IAskStorageRepository
{
    Task CreateAskAsync(Ask ask, CancellationToken cancellationToken);
    Task<GetAsksResponse> GetAsksAsync(GetAsksRequest request, CancellationToken cancellationToken);
    Task DeleteAsksAsync(DeleteAsksRequest request, CancellationToken cancellationToken);
}