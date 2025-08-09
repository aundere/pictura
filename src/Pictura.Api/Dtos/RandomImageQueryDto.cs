using Microsoft.AspNetCore.Mvc;

namespace Pictura.Api.Dtos
{
    public record RandomImageQueryDto
    {
        [FromQuery(Name = "tags")]
        [ModelBinder(BinderType = typeof(ModelBinders.CommaSeparatedModelBinder))]
        public IList<string> Tags { get; init; } = new List<string>();
    }
}
