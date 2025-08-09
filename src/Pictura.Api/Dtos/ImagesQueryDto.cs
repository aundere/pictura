using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Pictura.Api.Dtos
{
    public record ImagesQueryDto
    {
        [FromQuery(Name = "offset")]
        [Range(0, int.MaxValue, ErrorMessage = "Offset must be a non-negative integer.")]
        public int Offset { get; init; } = 0;
        
        [FromQuery(Name = "limit")]
        [Range(0, 100, ErrorMessage = "Limit must be between 0 and 100.")]
        public int Limit { get; init; } = 10;

        [FromQuery(Name = "tags")]
        [ModelBinder(BinderType = typeof(ModelBinders.CommaSeparatedModelBinder))]
        public IList<string> Tags { get; init; } = [];
    }
}
