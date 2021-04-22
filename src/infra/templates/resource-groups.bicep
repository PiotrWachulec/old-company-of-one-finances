param envType string
param location string

targetScope = 'subscription'

resource mgmtRg 'Microsoft.Resources/resourceGroups@2020-06-01' = {
  name: 'INZ-${envType}-RG'
  location: location
}
