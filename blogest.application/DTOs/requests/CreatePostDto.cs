using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blogest.application.DTOs.requests
{
    public record CreatePostDto(string Title,string Content,DateTime PublishDate);
}