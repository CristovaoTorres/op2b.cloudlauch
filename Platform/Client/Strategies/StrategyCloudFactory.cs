using Microsoft.Extensions.DependencyInjection;
using Platform.Client.Strategies;
using Platform.Shared.Models.Enums;
using System;

public class StrategyCloudFactory : IStrategyCloudFactory
{
    private readonly IServiceProvider serviceProvider;

    public StrategyCloudFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IStrategyCloud CreateStrategy(string cloudProvider)
    {
        if (cloudProvider == eCloudProvider.Azure.GetStringValue())
        {
            return serviceProvider.GetRequiredService<StrategyAZURE>();
        }
        else if (cloudProvider == eCloudProvider.AWS.GetStringValue())
        {
            return serviceProvider.GetRequiredService<StrategyAWS>();
        }
        else
        {
            throw new ArgumentException("Cloud provider not supported.");
        }
    }
}
