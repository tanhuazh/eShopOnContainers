﻿using eShopOnContainers.Core.Extensions;
using eShopOnContainers.Core.Helpers;

namespace eShopOnContainers.Core.Services.Marketing
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Models.Marketing;
    using RequestProvider;

    public class CampaignService : ICampaignService
    {
        private readonly IRequestProvider _requestProvider;

        public CampaignService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ObservableCollection<CampaignItem>> GetAllCampaignsAsync(string userId, string token)
        {
            UriBuilder builder = new UriBuilder(GlobalSetting.Instance.MarketingEndpoint);

            builder.Path = $"api/v1/campaigns/user/{userId}";

            string uri = builder.ToString();

            CampaignRoot campaign =
                await _requestProvider.GetAsync<CampaignRoot>(uri, token);

            if (campaign?.Data != null)
            {
                ServicesHelper.FixCatalogItemPictureUri(campaign?.Data);

                return campaign?.Data.ToObservableCollection();
            }

            return new ObservableCollection<CampaignItem>();
        }

        public async Task<CampaignItem> GetCampaignByIdAsync(int campaignId, string token)
        {
            UriBuilder builder = new UriBuilder(GlobalSetting.Instance.MarketingEndpoint);

            builder.Path = $"api/v1/campaigns/{campaignId}";

            string uri = builder.ToString();

            return await _requestProvider.GetAsync<CampaignItem>(uri, token);
        }
    }
}