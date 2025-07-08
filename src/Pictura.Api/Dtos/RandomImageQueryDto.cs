using Microsoft.AspNetCore.Mvc;

namespace Pictura.Api.Dtos
{
    public record RandomImageQueryDto
    {
        [FromQuery(Name = "tags")]
        [ModelBinder(BinderType = typeof(ModelBinders.CommaSeparatedModelBinder))]
        public IEnumerable<string> Tags { get; init; } = new HashSet<string>();
    }
}
