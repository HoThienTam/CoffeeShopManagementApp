using ApplicationCore.Extensions;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dtos;
using ImTools;
using Infrastructure.Data;
using Infrastructure.Models;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.ItemService
{
    public class AddItemCommandHandler : IRequestHandler<AddItemCommand, ItemDto>
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public AddItemCommandHandler(DataContext context, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        public async Task<ItemDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Item>(request.ItemDto);
            var uploadResult = new ImageUploadResult();
            if (request.ItemDto.ImageFile.Length > 0)
            {
                using (var stream = new MemoryStream(request.ItemDto.ImageFile))
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(item.Name, stream),
                        Transformation = new Transformation()
                            .Width(150).Height(100).Crop("fill").Gravity("face")
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            item.Image = uploadResult.Url.ToString();
            await _context.Items.AddAsync(item);

            if (await _context.SaveChangesAsync() > 0)
            {
                var itemDto = _mapper.Map<ItemDto>(item);
                return itemDto;
            }

            return null;

        }
    }
}
