param defaultResourceName string
param integrationResourceGroupName string
@secure()
param secretValue string

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: '${integrationResourceGroupName}-kv'
}

resource keyVaultSecret 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  name: '${defaultResourceName}-storage'
  parent: keyVault
  properties: {
    value: secretValue
  }
}

output keyVaultReference string = '@Microsoft.KeyVault(SecretUri=${keyVaultSecret.properties.secretUri})'
