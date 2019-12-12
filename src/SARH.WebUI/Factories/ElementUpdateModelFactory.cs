using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISOSA.SARH.Data.Domain.Common;
using ISOSA.SARH.Data.Repository;
using SARH.WebUI.Models.Common;

namespace SARH.WebUI.Factories
{
    public class ElementUpdateModelFactory : IElementUpdateModelFactory
    {

        private readonly IRepository<ElementUpdate> _repository;

        public ElementUpdateModelFactory(IRepository<ElementUpdate> repository)
        {
            _repository = repository;
        }


        public UpdateElementItem GetCatalogUpdateSync(int elementType)
        {
            UpdateElementItem model = new UpdateElementItem();
            var element = _repository.SearhItemsFor(p => p.ElementType.Equals(elementType));
            if (element.Any())
            {
                var data = element.FirstOrDefault().UpdateSync.HasValue ? $" {element.FirstOrDefault().UpdateSync.Value.ToShortDateString()}." : ".";
                model = new UpdateElementItem()
                {
                    ElementId = elementType,
                    ElementLastUpdate = $"última fecha de actualización{data}"
                };
            }
            return model;
        }
    }
}
