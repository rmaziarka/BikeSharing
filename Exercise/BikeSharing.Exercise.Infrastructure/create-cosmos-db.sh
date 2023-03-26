
# Variables for SQL API resources
uniqueId=$RANDOM
resourceGroupName="bikesharing-32003"
location='westeurope'
accountName="$resourceGroupName-cosmos"
workspaceName="$resourceGroupName-workspace"
databaseName='database'

availabilityContainerName='availability'
availabilityPartitionKey='//CityId'

rentalsContainerName='rentals'
rentalsPartitionKey='//ClientId'

clientsContainerName='clients'
clientsPartitionKey='//CityId'

subscriptionId=$(az account show --query id --output tsv)

# Create a resource group
az group create -n $resourceGroupName -l $location

# Create a Cosmos account for SQL API
az cosmosdb create \
    -n $accountName \
    -g $resourceGroupName \
    --default-consistency-level Session \
    --locations regionName=$location failoverPriority=0 isZoneRedundant=False \
    --capabilities EnableServerless

# Create a NoSQL API database
az cosmosdb sql database create \
    -a $accountName \
    -g $resourceGroupName \
    -n $databaseName

# Create Availability container
az cosmosdb sql container create \
    -a $accountName \
    -g $resourceGroupName \
    -d $databaseName \
    -n $availabilityContainerName \
    -p $availabilityPartitionKey \
    --idx @cosmos-index-policy.json

# Create Rental container
az cosmosdb sql container create \
    -a $accountName \
    -g $resourceGroupName \
    -d $databaseName \
    -n $rentalsContainerName \
    -p $rentalsPartitionKey \
    --idx @cosmos-index-policy.json

# Create Clients container
az cosmosdb sql container create \
    -a $accountName \
    -g $resourceGroupName \
    -d $databaseName \
    -n $clientsContainerName \
    -p $clientsPartitionKey \
    --idx @cosmos-index-policy.json

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
    -n 'Cosmos DB' \
    --export-to-resource-specific true \
    --logs "@log-analytics-diagnostic-logs.json" \
    --metrics '[{"category": "Requests","categoryGroup": null,"enabled": true,"retentionPolicy": {"enabled": false,"days": 0}}]' \
    --workspace $workspacePath