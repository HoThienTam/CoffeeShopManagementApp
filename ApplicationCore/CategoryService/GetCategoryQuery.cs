using Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.CategoryService
{
    public class GetCategoryQuery : IRequest<CategoryDto>
    {
        public GetCategoryQuery(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
}
