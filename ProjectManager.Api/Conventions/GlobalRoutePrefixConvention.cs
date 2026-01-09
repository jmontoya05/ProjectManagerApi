using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ProjectManager.Api.Conventions
{
    public sealed class GlobalRoutePrefixConvention(string prefix) : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix = new(new Microsoft.AspNetCore.Mvc.RouteAttribute(prefix));

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                if (controller.Selectors == null || controller.Selectors.Count == 0)
                    continue;

                foreach (var selector in controller.Selectors)
                {
                    if (selector.AttributeRouteModel == null)
                        continue;

                    selector.AttributeRouteModel =
                        AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
                }
            }
        }
    }
}
