using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.CategoryService
{
    public class GetCategoriesQuery :IRequest<IEnumerable<CategoryDto>>
    {
    }
}
