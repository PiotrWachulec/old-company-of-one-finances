param envType string
param location string
param passwordSalt string

var appServicePlanName  = 'INZ-${envType}-ASP'
var appServiceName      = 'INZ-${envType}-AppService'
var keyVaultName        = 'INZ-${envType}-KV'
var sqlSrvName          = 'INZ-${envType}-SQL-SRV'
var sqlDbName           = 'INZ-${envType}-SQL-DB'
var storageAccountName  = '${concat('inz', toLower(envType), 'sa')}'

var sqlSrvDbAdminLogin    = 'inz${toLower(envType)}dbsrvadmin'
var sqlSrvDbAdminPassword = '${concat('P', uniqueString(resourceGroup().id, passwordSalt), 't', '$%#')}'

var dbSrvAdminLoginSecretName         = 'DB-SRV-Admin-Login'
var dbSrvAdminLoginSecretPassword     = 'DB-SRV-Admin-Password'
var dbSrvAdminLoginSecretPasswordSalt = 'DB-SRV-Admin-Password-Salt-Generator'

resource appServicePlan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: appServicePlanName
  location: location
  kind: 'Linux'

  properties: {
    reserved: true
  }

  sku: {
    tier: 'Free'
    size: 'F1'
  }
}

resource appService 'Microsoft.Web/sites@2020-12-01' = {
  name: appServiceName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
  }
}

resource sqlServer 'Microsoft.Sql/servers@2020-11-01-preview' = {
  name: sqlSrvName
  location: location
  properties: {
    administratorLogin: sqlSrvDbAdminLogin
    administratorLoginPassword: sqlSrvDbAdminPassword
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2020-11-01-preview' = {
  name: sqlDbName
  location: location
  parent: sqlServer

  sku: {
    name: 'GP_S_Gen5'
    tier: 'GeneralPurpose'
    family: 'Gen5'
    capacity: 1
  }

  properties: {
    autoPauseDelay: 10
    maxSizeBytes: 1000000000
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
  }
}

resource stg 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      name: 'standard'
      family: 'A'
    }
    tenantId: subscription().tenantId
    accessPolicies:[
      
    ]
    enableSoftDelete: false
  }
}

resource dbSrvAdminLoginSecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  name: dbSrvAdminLoginSecretName
  parent: keyVault
  properties: {
    value: sqlSrvDbAdminLogin
  }
}

resource dbSrvAdminPasswordSecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  name: dbSrvAdminLoginSecretPassword
  parent: keyVault
  properties: {
    value: sqlSrvDbAdminPassword
  }
}

resource dbSrvAdminPasswordSalt 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  name: dbSrvAdminLoginSecretPasswordSalt
  parent: keyVault
  properties: {
    value: passwordSalt
  }
}
