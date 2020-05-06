using Infrastructure.Models;
using MediatR;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Queries
{
    public class GetByIdQuery<T, V> : IRequest<T> where T : BindableBase where V : BaseModel
    {
        public GetByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
