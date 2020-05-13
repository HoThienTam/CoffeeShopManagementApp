using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.CategoryService
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
