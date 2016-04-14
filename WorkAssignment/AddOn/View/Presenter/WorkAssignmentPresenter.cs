using System;
using System.Collections.Generic;
using System.Linq;
using Plugin_WorkAssignment.View.ViewAbstraction;
using Microsoft.Practices.Unity;
using ServicesAPI.Caching;
using WebServicesAPI.Configuration;
using ServicesAPI.Configuration;
using Utilities;
using Utilities.ErrorHandling;

namespace Plugin_WorkAssignment.View.Presenter
{
    public class WorkAssignmentPresenter : IWorkAssignmentPresenter
    {
        public IWorkAssignment View { get; set; }

        [InjectionConstructor]
        public WorkAssignmentPresenter(ICachingService cachingService, IConfigurationService configurationService)
        {
            _cachingService = cachingService;
            _configurationService = configurationService;
        }

        public void GetShops()
        {
            string shopDataSourceID = "";

            try
            {
                Mapping mapping = _configurationService.GetActivityMapping();
                shopDataSourceID = mapping.GetDatasourceIdByPrimaryKey(SyntempoConstants.SHOP);
            }
            catch (Exception e)
            {
                throw new MappingNotFoundException(e.Message);
            }

            this.View.ShopList = _cachingService.GetLookupItems(shopDataSourceID);

        }

        public void GetAreas()
        {
            string areaDataSourceID = "";

            try
            {
                Mapping mapping = _configurationService.GetActivityMapping();
                areaDataSourceID = mapping.GetDatasourceIdByPrimaryKey(SyntempoConstants.AREA);
            }
            catch (Exception e)
            {
                throw new MappingNotFoundException(e.Message);
            }

            this.View.AreaList = _cachingService.GetLookupItems(areaDataSourceID);
        }

        private readonly ICachingService _cachingService;
        /// <summary>
        /// Service to retrieve activity field mappings
        /// </summary>
        private static IConfigurationService _configurationService;

    }
}