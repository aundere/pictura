using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Pictura.Api.Dtos
{
    public record ImagesQueryDto
    {
        [FromQuery(Name = "limit")]
        [Range(0, 100, ErrorMessage = "Limit must be between 0 and 100.")]
        public int Limit { get; init; } = 10;

        [FromQuery(Name = "tags")]
        [ModelBinder(BinderType = typeof(ModelBinders.CommaSeparatedModelBinder))]
        public IEnumerable<string> Tags { get; init; } = new HashSet<string>();
    }
}
