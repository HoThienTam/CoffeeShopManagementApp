using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Queries
{
    public class GetCategoriesQuery :IRequest<IEnumerable<CategoryDto>>
    {
    }
}
