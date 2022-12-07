param defaultResourceName string
param location string
param environmentName string
param integrationResourceGroup string
param storageTables array
param containerVersion string

resource containerAppEnvironments 'Microsoft.App/managedEnvironments@2022-06-01-preview' existing = {
  name: '${integrationResourceGroup}-env'
  scope: resourceGroup(integrationResourceGroup)
}
resource appConfiguration 'Microsoft.AppConfiguration/configurationStores@2022-05-01' existing = {
  name: '${integrationResourceGroup}-cfg'
  scope: resourceGroup(integrationResourceGroup)
}
resource containerRegistry 'Microsoft.ContainerRegistry/registries@2022-02-01-preview' existing = {
  name: 'ekereg'
  scope: resourceGroup('Containers')
}

resource identity 'Microsoft.ManagedIdentity/userAssignedIdentities@2022-01-31-preview' = {
  name: '${defaultResourceName}-id'
  location: location
}

module allRoleAssignments 'all-role-assignments.bicep' = {
  name: 'allRoleAssignmentsModule'
  params: {
    containerAppPrincipalId: identity.properties.principalId
    integrationResourceGroupName: integrationResourceGroup
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: uniqueString(defaultResourceName)
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'

}
resource storageAccountTableService 'Microsoft.Storage/storageAccounts/tableServices@2022-09-01' = {
  name: 'default'
  parent: storageAccount
}
resource storageAccountTable 'Microsoft.Storage/storageAccounts/tableServices/tables@2022-09-01' = [for table in storageTables: {
  name: table
  parent: storageAccountTableService
}]

resource azureContainerApp 'Microsoft.App/containerApps@2022-03-01' = {
  dependsOn: [
    allRoleAssignments
  ]
  name: '${defaultResourceName}-aca'
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${identity.id}': {}
    }
  }
  properties: {
    managedEnvironmentId: containerAppEnvironments.id

    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: false
        targetPort: 80
        transport: 'auto'
        allowInsecure: false
        traffic: [
          {
            weight: 100
            latestRevision: true
          }
        ]
      }
      secrets: [
        {
          name: 'container-registry-password'
          value: containerRegistry.listCredentials().passwords[0].value
        }
      ]
      registries: [
        {
          server: containerRegistry.name
          username: containerRegistry.properties.loginServer
          passwordSecretRef: 'container-registry-password'
        }
      ]
      dapr: {
        enabled: true
        appPort: 80
        appId: 'blackjack-players-service'
      }
    }
    template: {
      containers: [
        {
          image: 'ekereg.azurecr.io/blackjack-players-api:${containerVersion}'
          name: 'blackjack-players-service'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'Azure__StorageAccount'
              value: storageAccount.name
            }
            {
              name: 'Azure__AzureAppConfiguration'
              value: appConfiguration.properties.endpoint
            }
            {
              name: 'AllowedCorsOrigins'
              value: 'http://localhost:4200;https://blackjack.hexmaster.nl'
            }
          ]

        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 6
        rules: [
          {
            name: 'http-rule'
            http: {
              metadata: {
                concurrentRequests: '30'
              }
            }
          }
        ]
      }
    }
  }
}
