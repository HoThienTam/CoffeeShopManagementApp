using ApplicationCore.Queries;
using AutoMapper;
using Infrastructure.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, bool>
    {
        private IUserRepository _repo;
        public GetUserQueryHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByIdAsync(request.Id);

            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
