# DoMyThing
Authomize your things online. Crawl websites and process them on your behalf.

# Componenets
- Azure Function App
- Azure Service Bus
- Azure Blob Storage

# Functions
## Download Subtitle Function
1. Triggered by a message into azure service bus queue. 
2. Crawl website to get download link of proper subtitle
3. Download subtitle
4. Upload file to Blob storage
5. Send a message containing saved blob storage name to queue


# Deployment

Function app is isolated. Deploy accordingly. 
Queues in Azure Service Bus should have been created before running application

## Required configurations to be set to Azure function app when deployed
```json
 "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "BlobStorage": "<Azure Storage Connection String>",
    "BlobStorageSubtitleContainer": "subtitles",
    "DownloadSubtitleQueueName": "download-subtitle",
    "SubtitleDownloadedQueueName": "subtitle-downloaded",
    "ServiceBusConnection": "<Azure Service Bus Connection String>"
  }
```