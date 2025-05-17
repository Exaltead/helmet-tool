# SETUP

Important, as this uses Entra ID to authenticate, setting up correct permissions is needed. 

```
# Optional, as the ID is alwas the 00000000-0000-0000-0000-000000000002 
az cosmosdb sql role definition list --resource-group <RESROUCE_GROUP> --account-name <COSMOS_ACCOUNTN_NAME>

#Returns the scope
az cosmosdb show --resource-group <RESROUCE_GROUP> --name <COSMOS_ACCOUNT_NAME> --query "{id:id}"

az cosmosdb sql role assignment create   --resource-group <RESOURCE_GROUP>   --account-name <COSMOS_ACCOUNT_NAME>   --role-definition-id "00000000-0000-0000-0000-000000000002"     --principal-id "<YOUR_PRINCIPAL>"   --scope "<SCOPE>"
```