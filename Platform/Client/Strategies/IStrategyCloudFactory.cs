using Platform.Client.Strategies;

public interface IStrategyCloudFactory
{
    IStrategyCloud CreateStrategy(string cloudProvider);
}
