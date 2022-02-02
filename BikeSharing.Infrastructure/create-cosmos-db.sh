
# Variables for SQL API resources
uniqueId=$RANDOM
resourceGroupName="br" #bike-rental
location='westeurope'
accountName="$resourceGroupName-$uniqueId-cosmos"
workspaceName="$resourceGroupName-$uniqueId-workspace"
databaseName='case1'

bikesContainerName='bikes'
bikesPartitionKey='//CityId'

rentalsContainerName='rentals'
rentalsPartitionKey='//UserId'

subscriptionId=$(az account show --query id --output tsv)

## Create a resource group
az group create -n $resourceGroupName -l $location

# Create a Cosmos account for SQL API
az cosmosdb create \
    -n $accountName \
    -g $resourceGroupName \
    --default-consistency-level Eventual \
    --locations regionName=$location failoverPriority=0 isZoneRedundant=False \
    --capabilities EnableServerless

# Create a SQL API database
az cosmosdb sql database create \
    -a $accountName \
    -g $resourceGroupName \
    -n $databaseName

# Create a SQL API container
az cosmosdb sql container create \
    -a $accountName \
    -g $resourceGroupName \
    -d $databaseName \
    -n $bikesContainerName \
    -p $bikesPartitionKey

# Create a SQL API container
az cosmosdb sql container create \
    -a $accountName \
    -g $resourceGroupName \
    -d $databaseName \
    -n $rentalsContainerName \
    -p $rentalsPartitionKey


# Create Log Analytics Workspace
az monitor log-analytics workspace create \
    -g $resourceGroupName \
    -n $workspaceName \
    -l $location

resourceName="//subscriptions\\$subscriptionId\\resourceGroups\\$resourceGroupName\\providers\\Microsoft.DocumentDb\\databaseAccounts\\$accountName"
workspacePath="//subscriptions\\$subscriptionId\\resourcegroups\\$resourceGroupName\\providers\\microsoft.operationalinsights\\workspaces\\$workspaceName"

# Connect Log Analytics Workspace to Cosmos DB
az monitor diagnostic-settings create \
    --resource $resourceName \
    -n 'Cosmos DB' --debug  \
    --export-to-resource-specific true \
    --logs "@log-analytics-diagnostic-logs.json" \
    --metrics '[{"category": "Requests","categoryGroup": null,"enabled": true,"retentionPolicy": {"enabled": false,"days": 0}}]' \
    --workspace $workspacePath