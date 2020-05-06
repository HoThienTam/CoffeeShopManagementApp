using Infrastructure.Models;
using MediatR;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Queries
{
    public class GetAllQuery<T, V> : IRequest<IEnumerable<T>> where T : BindableBase where V : BaseModel
    {
    }
}
