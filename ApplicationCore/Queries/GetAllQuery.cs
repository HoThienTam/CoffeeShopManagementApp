using Infrastructure.Models;
using MediatR;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Queries
{
    public class GetAllQuery<T, V> : IRequest<IEnumerable<T>> where T : BindableBase where V : BaseModel
    {
        public GetAllQuery(params Expression<Func<V, object>>[] includes)
        {
            Includes = includes;
        }
        public Expression<Func<V, object>>[] Includes { get; set; }
    }
}
