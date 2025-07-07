using Pictura.Api.Entities;

namespace Pictura.Api.Services
{
    public class ImagesStorage
    {
        private readonly Dictionary<Guid, ImageEntity> _images = new Dictionary<Guid, ImageEntity>();

        public IReadOnlyDictionary<Guid, ImageEntity> Images => this._images;

        public void AddImage(ImageEntity image)
        {
            this._images[image.Id] = image;
        }
    }
}
