using Infrastructure.Models;
using MediatR;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Queries
{
    public class GetByIdQuery<T, V> : IRequest<T> where T : BindableBase where V : BaseModel
    {
        public GetByIdQuery(Guid id, params Expression<Func<V, object>>[] includes)
        {
            Id = id;
            Includes = includes;
        }
        public Expression<Func<V, object>>[] Includes { get; set; }
        public Guid Id { get; set; }
    }
}
