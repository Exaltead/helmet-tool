param appName string

var uniqueSuffix = uniqueString(resourceGroup().id)
var location = resourceGroup().location
var funcAppName = '${appName}-func-${uniqueSuffix}'
var hostingAccountName = '${appName}hs${uniqueSuffix}'

resource hostingStorageAccount 'Microsoft.Storage/storageAccounts@2024-01-01' = {
  name: hostingAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    supportsHttpsTrafficOnly: true
  }
}

resource hostingStorageAccountBlobService 'Microsoft.Storage/storageAccounts/blobServices@2024-01-01' = {
  parent: hostingStorageAccount
  name: 'default'
}

resource backendContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2024-01-01' = {
  parent: hostingStorageAccountBlobService
  name: 'backend'
  properties: {
    publicAccess: 'None'
  }
}

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2025-02-01' = {
  name: '${appName}-la-${uniqueSuffix}'
  location: location
  properties: {
    retentionInDays: 30
    sku: {
      name: 'Standalone'
    }
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${appName}-ai-${uniqueSuffix}'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalytics.id
    DisableLocalAuth: true
  }
}

resource funcAppIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2024-11-30' = {
  name: '${funcAppName}-identity-${uniqueSuffix}'
  location: location
}

resource storageBlobDataReaderRole 'Microsoft.Authorization/roleDefinitions@2022-04-01' existing = {
  name: '2a2b9908-6ea1-4ae2-8e65-a410df84e7d1'
  scope: subscription()
}

resource readAssignmentToHostBucket 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: hostingStorageAccount
  name: guid(resourceGroup().id, hostingStorageAccount.id, 'readAssignmentToHostBucket')
  properties: {
    principalId: funcAppIdentity.properties.principalId
    roleDefinitionId: storageBlobDataReaderRole.id
    principalType: 'ServicePrincipal'
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: '${appName}-plan-${uniqueSuffix}'
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {
    reserved: true
  }
}

resource functionApp 'Microsoft.Web/sites@2024-04-01' = {
  name: funcAppName
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${funcAppIdentity.id}': {}
    }
  }
  properties: {
    reserved: true
    serverFarmId: hostingPlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNET-ISOLATED|8.0'
      appSettings: [
        { name: 'FUNCTIONS_EXTENSION_VERSION', value: '~4' }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${hostingAccountName};AccountKey=${hostingStorageAccount.listKeys().keys[0].value};EndpointSuffix=core.windows.net'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${hostingAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${hostingStorageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(funcAppName)
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '${hostingStorageAccount.properties.primaryEndpoints.blob}${backendContainer.name}/app.zip'
        }
        { name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED', value: '0' }
        {
          name: 'CosmosDBConnectionString'
          value: 'foobar'
        }
        { name: 'DatabaseName', value: 'foobar' }
        {
          name: 'SecretKey'
          value: 'foobar'
        }
        { name: 'APPLICATIONINSIGHTS_CONNECTION_STRING', value: applicationInsights.properties.ConnectionString }
        { name: 'WEBSITE_RUN_FROM_PACKAGE_BLOB_MI_RESOURCE_ID', value: funcAppIdentity.id }
      ]
    }
  }
}
